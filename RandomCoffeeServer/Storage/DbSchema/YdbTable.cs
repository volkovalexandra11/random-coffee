namespace RandomCoffeeServer.Storage.DbSchema;

public class YdbTable
{
    public string TableName { get; init; }
    public YdbColumn[] Columns { get; init; }
    public int[] PrimaryKeyColumns { get; init; }
    public YdbIndex[]? Indexes { get; init; }

    public YdbIndex IndexByColumn(string column)
    {
        if (Indexes is null)
            throw new InvalidProgramException();

        return Indexes.Single(index => index.IndexColumns.Length == 1 && Columns[index.IndexColumns[0]].Name == column);
    }
}