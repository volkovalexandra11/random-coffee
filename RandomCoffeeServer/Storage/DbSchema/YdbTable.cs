using System.Text;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

namespace RandomCoffeeServer.Storage.DbSchema;

public class YdbTable
{
    public string TableName { get; init; }
    public YdbColumn[] Columns { get; init; }
    public int[] PrimaryKeyColumns { get; init; }
    public YdbIndex[]? Indexes { get; init; }

    public YdbTable()
    {
        var properties = GetType().GetProperties();
    }

    public string ToDdl()
    {
        var ddl = new StringBuilder($"CREATE TABLE {TableName} (\n");
        AddColumnList(ddl);
        ddl.Append(',');
        foreach (var index in Indexes ?? Array.Empty<YdbIndex>())
        {
            ddl.Append($"  {index.ToDdl(Columns)},\n");
        }
        ddl.Append($"  PRIMARY KEY ({string.Join(", ", PrimaryKeyColumns.Select(i => Columns[i].Name))})\n");
        return ddl.Append(");").ToString();
    }

    public string ToDelete()
    {
        return $"DROP TABLE {TableName};";
    }
    
    public string ToDeclare()
    {
        var declare = new StringBuilder("DECLARE ");
        declare.Append(YdbConverter.DataParamName);
        declare.Append(" AS List<Struct<\n");
        AddColumnList(declare);
        declare.Append(">>;");

        return declare.ToString();
    }

    private StringBuilder AddColumnList(StringBuilder sb)
    {
        foreach (var column in Columns)
        {
            sb.Append($"  {column.ToDdl()},\n");
        }
        return sb.Remove(sb.Length - 2, 2);
    }

    private readonly string declareStatement;
}