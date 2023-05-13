namespace RandomCoffeeServer.Controllers.ApiControllers.QueryStrings;

public abstract class QueryStringParametersBase
{
    private const int MaxPageSize = 20;
    private const int MinPageSize = 1;
    private int pageSize = 5;
    private int page = 1;

    public int Page
    {
        get => page;
        set => page = value < 1 ? page : value;
    }

    public int PageSize
    {
        get => pageSize;
        set
        {
            if (value < MinPageSize)
                pageSize = MinPageSize;
            if (value > MaxPageSize)
                pageSize = MaxPageSize;
            pageSize = value;
        }
    }
}