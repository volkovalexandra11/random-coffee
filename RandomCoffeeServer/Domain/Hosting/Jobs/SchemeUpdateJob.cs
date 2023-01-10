using RandomCoffeeServer.Storage.DbSchema;
using RandomCoffeeServer.Storage.YandexCloud.Ydb;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;
using Ydb.Sdk;

namespace RandomCoffeeServer.Domain.Hosting.Jobs;

public class SchemeUpdateJob
{
    public SchemeUpdateJob(YdbService ydbService)
    {
        this.ydbService = ydbService;
    }

    public async Task UpdateScheme(CancellationToken cancellationToken)
    {
        var tables = Schema.Tables;
        var queries = tables.Select(table => table.ToDdl());

        try
        {
            foreach (var query in queries)
            {
                await ydbService.ExecuteScheme(query);
                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();
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
                    await DeleteScheme(cancellationToken);
                    await UpdateScheme(cancellationToken);
                    return;
                }
            }

            throw;
        }
    }

    public async Task DeleteScheme(CancellationToken cancellationToken)
    {
        var tables = Schema.Tables;
        var queries = tables.Select(table => table.ToDelete());
        foreach (var query in queries)
        {
            await ydbService.ExecuteScheme(query);

            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();
        }
    }

    private readonly YdbService ydbService;
    private const int TypeAnnotationIssueCode = 1030;
}