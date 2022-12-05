namespace RandomCoffeeServer.DbSchema;

public class YdbIndex
{
    public string IndexName { get; init; }
    public bool IsAsync { get; init; } = false;
    public int[] IndexColumns { get; init; }
    public int[]? CoverColumns { get; init; }

    public string ToDdl(YdbColumn[] columns)
    {
        return $"INDEX {IndexName} GLOBAL" +
               (IsAsync ? " ASYNC" : " SYNC") +
               $" ON ({string.Join(", ", IndexColumns.Select(i => columns[i].Name))})" +
               (CoverColumns != null ? $" COVER ({string.Join(", ", CoverColumns.Select(i => columns[i].Name))})" : "");
    }
}