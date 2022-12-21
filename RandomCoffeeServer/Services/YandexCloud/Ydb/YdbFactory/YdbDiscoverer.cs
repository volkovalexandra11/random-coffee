using System.Web;
using Yandex.Cloud;
using Yandex.Cloud.Credentials;
using Yandex.Cloud.Ydb.V1;

namespace RandomCoffeeServer.Services.YandexCloud.Ydb.YdbFactory;

public class YdbDiscoverer
{
    public YdbDiscoverer(ICredentialsProvider credentialsProvider)
    {
        databaseService = new Sdk(credentialsProvider).Services.Ydb.DatabaseService;
    }

    public async Task<YdbPath> Discover(string folderId, string dbWithPath)
    {
        var dbs = await databaseService.ListAsync(new ListDatabasesRequest { FolderId = folderId });

        var (dbName, dbPath) = GetDbName(dbWithPath);
        var db = dbs.Databases.Single(d => d.Name == dbName);
        var dbUri = new Uri(db.Endpoint);
        var endpoint = dbUri.GetLeftPart(UriPartial.Path).TrimEnd('/');
        var database = HttpUtility.ParseQueryString(dbUri.Query)["database"]!;
        return new YdbPath { Endpoint = endpoint, Database = database, Path = string.Join('/', dbPath) };
    }

    private (string dbName, string[] dbPath) GetDbName(string dbWithPath)
    {
        var parts = dbWithPath.Split('/', 2);
        return (parts[0], parts[1..]);
    }

    private readonly DatabaseService.DatabaseServiceClient databaseService;
}