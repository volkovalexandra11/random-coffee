using RandomCoffee.schema;
using Ydb.Sdk;

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

        try
        {
            foreach (var query in queries)
            {
                await ydbService.ExecuteScheme(query);
            }
        }
        catch (StatusUnsuccessfulException ex)
        {
            if (ex.Status.Issues.First().IssueCode == TypeAnnotationIssueCode)
            {
                Console.WriteLine("Conflict when updating database schema. RESET REMOTE DATABASE? [YES/NO]");
                var answer = Console.ReadLine();
                if (answer == "YES")
                {
                    await DeleteScheme();
                    await UpdateScheme();
                    return;
                }
            }
            throw;
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
    private const int TypeAnnotationIssueCode = 1030;
}