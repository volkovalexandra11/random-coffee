using System.Text;

namespace RandomCoffee.schema;

public class YdbTable
{
    public string TableName { get; init; }
    public YdbColumn[] Columns { get; init; }
    public int[] PrimaryKeyColumns { get; init; }
    public YdbIndex[]? Indexes { get; init; }

    public string ToDdl()
    {
        var ddl = new StringBuilder($"CREATE TABLE {TableName} (\n");
        foreach (var column in Columns)
        {
            ddl.Append($"  {column.Name} {column.Type},\n");
        }
        foreach (var index in Indexes ?? Array.Empty<YdbIndex>())
        {
            ddl.Append($"  {index.ToDdl(Columns)},\n");
        }
        ddl.Append($"  PRIMARY KEY ({string.Join(", ", PrimaryKeyColumns.Select(i => Columns[i].Name))})\n");
        return ddl.Append(");").ToString();
    }

    public string ToDelete()
    {
        return $"DELETE TABLE {TableName};";
    }
}