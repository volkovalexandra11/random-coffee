using Ydb.Sdk;
using Ydb.Sdk.Auth;
using Ydb.Sdk.Client;
using Ydb.Sdk.Table;
using Ydb.Sdk.Value;

namespace RandomCoffee;

public class YdbService
{
    public YdbService(YdbPath ydbPath, ICredentialsProvider credentialsProvider, ILoggerFactory loggerFactory)
    {
        var config = new DriverConfig(ydbPath.Endpoint, ydbPath.Database, credentialsProvider);
        ydbDriver = new Driver(config, loggerFactory);
        ydbDriver.Initialize().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    public async Task<ExecuteSchemeQueryResponse> ExecuteScheme(string query)
    {
        using var tableClient = new TableClient(ydbDriver, new TableClientConfig());
        var response = await tableClient.SessionExec(async sesstion => await sesstion.ExecuteSchemeQuery(query, new ExecuteSchemeQuerySettings() { OperationTimeout = TimeSpan.FromMinutes(1) }));
        response.Status.EnsureSuccess();
        return (ExecuteSchemeQueryResponse)response;
    }

    public async Task<IResponse> Execute(string query, Dictionary<string, YdbValue> @params)
    {
        using var tableClient = new TableClient(ydbDriver, new TableClientConfig());
        var response = await tableClient.SessionExec(async session => await session.ExecuteDataQuery(
            query,
            TxControl.BeginSerializableRW().Commit(),
            @params,
            new ExecuteDataQuerySettings()
        ));
        response.Status.EnsureSuccess();
        return (ExecuteDataQueryResponse)response;
    }

    private readonly Driver ydbDriver;
}