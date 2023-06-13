using System.Text;
using RandomCoffeeServer.Storage.DbSchema;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

public class Query
{
    private readonly YdbTable table;

    private string method;
    private string[]? columns;
    private YdbIndex? view;
    private Dictionary<string, YdbValue>? dataValue;

    private Dictionary<string, YdbValue>? whereParams;
    private Dictionary<string, YdbValue>? setParams;

    public Query(YdbTable table)
    {
        this.table = table;
    }

    public Query Select(params string[] columns)
    {
        method = "SELECT";
        this.columns = columns.Length > 0 ? columns : null;
        return this;
    }

    public Query Delete()
    {
        method = "DELETE";
        return this;
    }

    public Query Insert(Dictionary<string, YdbValue> value)
    {
        method = "INSERT";
        dataValue = value;
        return this;
    }

    public Query Replace(Dictionary<string, YdbValue> value)
    {
        method = "REPLACE";
        dataValue = value;
        return this;
    }

    public Query Update(Dictionary<string, YdbValue> setParameters, Dictionary<string, YdbValue> whereParameters)
    {
        method = "UPDATE";
        var query = this;
        foreach (var (column, value) in setParameters)
        {
            query = Set(column, value);
        }
        foreach (var (column, value) in whereParameters)
        {
            query = Where(column, value);
        }
        return query;
    }

    public Query ViewByColumn(string columnName)
    {
        this.view = table.IndexByColumn(columnName);
        return this;
    }

    public Query Where(string column, YdbValue value) //TODO(Cockamamie): неудобно передавать несколько столбцов
    {
        whereParams ??= new Dictionary<string, YdbValue>();
        whereParams[column] = value;
        return this;
    }

    public Query Where(Dictionary<string, YdbValue> filterValues)
    {
        whereParams ??= new Dictionary<string, YdbValue>();
        foreach (var (column, value) in filterValues)
        {
            whereParams[column] = value;
        }

        return this;
    }

    public Query Set(string column, YdbValue value) //TODO(Cockamamie): неудобно передавать несколько столбцов
    {
        setParams ??= new Dictionary<string, YdbValue>();
        setParams[column] = value;
        return this;
    }

    public (string query, Dictionary<string, YdbValue> @params) ToYdbQuery()
    {
        var builder = new StringBuilder();

        if (method is "INSERT" or "REPLACE")
            builder.Append(table.AsTableDeclare()).Append('\n');
        if (whereParams != null)
            builder.Append(string.Join("\n", ParamsToDeclares(whereParams, "where"))).Append('\n');
        if (setParams != null)
            builder.Append(string.Join("\n", ParamsToDeclares(setParams, "set"))).Append('\n');

        builder.Append(method);
        if (method is "SELECT")
        {
            if (columns != null)
                builder.Append(' ').Append(string.Join(", ", columns));
            else
                builder.Append(' ').Append('*');
        }

        if (method is "SELECT" or "DELETE")
            builder.Append(" FROM");
        else if (method != "UPDATE")
            builder.Append(" INTO");

        builder.Append(' ').Append(table.TableName);
        if (view != null)
            builder.Append(" VIEW ").Append(view.IndexName);

        if (setParams != null)
        {
            builder.Append(" SET ");
            builder.Append(string.Join(
                    " , ",
                    setParams.Select(
                        columnAndValue => $"{columnAndValue.Key} = {ToSetParamName(columnAndValue.Key)}")
                )
            );
        }

        if (whereParams != null)
        {
            builder.Append(" WHERE ");
            builder.Append(string.Join(
                    " AND ", // TODO(Cockamamie): нужна опция для OR
                    whereParams.Select(
                        columnAndValue => $"{columnAndValue.Key} = {ToWhereParamName(columnAndValue.Key)}")
                )
            );
        }

        if (method is "INSERT" or "REPLACE")
        {
            builder.Append(" SELECT * FROM AS_TABLE($data)");
        }

        builder.Append(';');

        var query = builder.ToString();
        var allParams = MakeAllParams(dataValue, ("where", whereParams), ("set", setParams));
        return (query, allParams);
    }

    public async Task ExecuteNonData(YdbService ydb)
    {
        var (query, @params) = ToYdbQuery();
        var response = await ydb.Execute(query, @params);
        response.Status.EnsureSuccess();
    }

    public async Task<IReadOnlyList<ResultSet.Row>> ExecuteData(YdbService ydb)
    {
        var (query, @params) = ToYdbQuery();
        var response = await ydb.Execute(query, @params);
        response.Status.EnsureSuccess();
        var result = ((ExecuteDataQueryResponse)response).Result;
        return result.ResultSets.Count > 0
            ? result.ResultSets[0].Rows
            : new List<ResultSet.Row>();
    }

    private IEnumerable<string> ParamsToDeclares(Dictionary<string, YdbValue> @params, string? paramPrefix = null)
    {
        foreach (var (name, value) in @params)
        {
            yield return $"DECLARE ${(paramPrefix != null ? $"{paramPrefix}_" : "")}{name} AS {value.TypeId};";
        }
    }

    private static Dictionary<string, YdbValue> MakeAllParams(
        Dictionary<string, YdbValue>? dataParam,
        params (string paramPrefix, Dictionary<string, YdbValue>? @params)[] paramGroups)
    {
        var allParams = new Dictionary<string, YdbValue>();

        if (dataParam != null)
        {
            foreach (var (dataParamName, dataParamValue) in dataParam.ToDataParams())
                allParams[dataParamName] = dataParamValue;
        }

        foreach (var (paramPrefix, @params) in paramGroups
                     .Where(group => group.@params != null))
        {
            foreach (var (paramName, paramValue) in @params!)
            {
                allParams[$"${paramPrefix}_{paramName}"] = paramValue;
            }
        }

        return allParams;
    }

    private string ToWhereParamName(string column)
    {
        return $"$where_{column}";
    }

    private string ToSetParamName(string column)
    {
        return $"$set_{column}";
    }
}