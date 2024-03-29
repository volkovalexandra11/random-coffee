﻿using System.Text;
using RandomCoffeeServer.Storage.DbSchema;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

public static class YdbTableExtensions
{
    public static string ToDdl(this YdbTable table)
    {
        var ddl = new StringBuilder($"CREATE TABLE {table.TableName} (\n");
        AddColumnList(table, ddl);
        ddl.Append(',');
        foreach (var index in table.Indexes ?? Array.Empty<YdbIndex>())
        {
            ddl.Append($"  {index.ToDdl(table.Columns)},\n");
        }

        ddl.Append(
            $"  PRIMARY KEY ({string.Join(", ", table.PrimaryKeyColumns.Select(i => table.Columns[i].Name))})\n");
        return ddl.Append(");").ToString();
    }

    private static StringBuilder AddColumnList(YdbTable table, StringBuilder sb)
    {
        foreach (var column in table.Columns)
        {
            sb.Append($"  {column.ToDdl()},\n");
        }

        return sb.Remove(sb.Length - 2, 2);
    }

    public static string ToDelete(this YdbTable table)
    {
        return $"DROP TABLE {table.TableName};";
    }

    public static string AsTableDeclare(this YdbTable tableSchema, string? paramName = null)
    {
        paramName ??= "data";
        var sb = new StringBuilder($"DECLARE ${paramName} AS List<Struct<\n");
        foreach (var column in tableSchema.Columns)
        {
            sb.Append($"  {column.Name}: {column.Type},\n");
        }

        sb.Remove(sb.Length - 2, 2);
        sb.Append(">>;");
        return sb.ToString();
    }

    public static Query Select(this YdbTable table, params string[]? columns)
    {
        return new Query(table)
            .Select(columns);
    }

    public static Query Delete(this YdbTable table)
    {
        return new Query(table)
            .Delete();
    }

    public static Query Insert(this YdbTable table, Dictionary<string, YdbValue> value)
    {
        return new Query(table)
            .Insert(value);
    }

    public static Query Replace(this YdbTable table, Dictionary<string, YdbValue> value)
    {
        return new Query(table)
            .Replace(value);
    }

    public static Query Update(
        this YdbTable table,
        Dictionary<string, YdbValue> setParameters,
        Dictionary<string, YdbValue> whereParameters)
    {
        return new Query(table)
            .Update(setParameters, whereParameters);
    }
}