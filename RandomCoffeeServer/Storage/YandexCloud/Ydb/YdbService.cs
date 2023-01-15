using System.Reflection;
using RandomCoffeeServer.Storage.YandexCloud.Ydb.YdbFactory;
using Ydb.Sdk;
using Ydb.Sdk.Auth;
using Ydb.Sdk.Client;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.YandexCloud.Ydb;

public class YdbService
{
    public YdbService(YdbPath ydbPath, ICredentialsProvider credentialsProvider, ILoggerFactory loggerFactory)
    {
        this.ydbPath = ydbPath;
        
        var config = new DriverConfig(ydbPath.Endpoint, ydbPath.Database, credentialsProvider);
        ydbDriver = new Driver(config, loggerFactory);
        ydbDriver.Initialize().ConfigureAwait(false).GetAwaiter().GetResult();
        
        tableClientConfig = new TableClientConfig(GetSessionPoolConfig());
    }

    public async Task<ExecuteSchemeQueryResponse> ExecuteScheme(string query)
    {
        using var tableClient = new TableClient(ydbDriver, tableClientConfig);
        var response = await tableClient.SessionExec(async sesstion => await sesstion.ExecuteSchemeQuery(
            GetQueryInDirectory(query),
            new ExecuteSchemeQuerySettings() { OperationTimeout = TimeSpan.FromMinutes(1) })
        );
        return (ExecuteSchemeQueryResponse)response;
    }

    public async Task<IResponse> Execute(string query, Dictionary<string, YdbValue> @params)
    {
        using var tableClient = new TableClient(ydbDriver, tableClientConfig);
        return await tableClient.SessionExec(async session => await session.ExecuteDataQuery(
            GetQueryInDirectory(query),
            TxControl.BeginSerializableRW().Commit(),
            @params,
            new ExecuteDataQuerySettings()
        ));
    }

    private string GetQueryInDirectory(string query)
    {
        return $"PRAGMA TablePathPrefix('{ydbPath.Database}/{ydbPath.Path}');\n{query}";
    }

    private SessionPoolConfig GetSessionPoolConfig()
    {
        var sessionConfig = new SessionPoolConfig();
        
        // lol this is bad but default timeout of 1s is laughable and causes huge problems to devs 
        typeof(SessionPoolConfig)
            .GetField("<CreateSessionTimeout>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(sessionConfig, TimeSpan.FromSeconds(6));

        return sessionConfig;
    }

    private readonly YdbPath ydbPath;
    private readonly Driver ydbDriver;
    private readonly TableClientConfig tableClientConfig;
}