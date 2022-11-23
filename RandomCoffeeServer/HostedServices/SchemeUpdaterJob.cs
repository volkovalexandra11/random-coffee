using RandomCoffee.schema;

namespace RandomCoffee.Services;

public class SchemeUpdaterJob : IHostedService
{
    public SchemeUpdaterJob(YdbService ydbService)
    {
        this.ydbService = ydbService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return UpdateScheme();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
        // throw new NotImplementedException();
    }

    public async Task UpdateScheme()
    {
        var tables = Schema.Tables;
        var queries = tables.Select(table => table.ToDdl());
        foreach (var query in queries)
        {
            await ydbService.ExecuteScheme(query);
        }
    }

    public async Task DeleteScheme()
    {
        var tables = Schema.Tables;
        var queries = tables.Select(table => table.ToDelete());
        foreach (var query in queries)
        {
            await ydbService.ExecuteScheme(query);
        }
    }

    private readonly YdbService ydbService;
}