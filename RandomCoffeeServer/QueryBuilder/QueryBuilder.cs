using System.Text;
using RandomCoffee.schema;

namespace RandomCoffee.QueryBuilder;

public static class QueryBuilder
{
    public static string AsTableDeclare(YdbTable tableSchema, string? paramName = null)
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
    
    // public static string AsTableUpsert()
}