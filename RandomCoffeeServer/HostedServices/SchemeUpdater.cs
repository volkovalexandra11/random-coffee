﻿using RandomCoffee.schema;
using Ydb.Sdk;

namespace RandomCoffee.Services;

public class SchemeUpdater
{
    public SchemeUpdater(YdbService ydbService)
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