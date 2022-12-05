using System.Web;
using Yandex.Cloud;
using Yandex.Cloud.Credentials;
using Yandex.Cloud.Ydb.V1;

namespace RandomCoffee;

public class YdbDiscoverer
{
    public YdbDiscoverer(ICredentialsProvider credentialsProvider)
    {
        databaseService = new Sdk(credentialsProvider).Services.Ydb.DatabaseService;
    }

    public async Task<YdbPath> Discover(string folderId, string dbSuffix)
    {
        var dbs = await databaseService.ListAsync(new ListDatabasesRequest { FolderId = folderId });

        var dbName = GetDbName(dbSuffix);
        var db = dbs.Databases.Single(d => d.Name == dbName);
        var dbUri = new Uri(db.Endpoint);
        var endpoint = dbUri.GetLeftPart(UriPartial.Path).TrimEnd('/');
        var database = HttpUtility.ParseQueryString(dbUri.Query)["database"]!;
        return new YdbPath { Endpoint = endpoint, Database = database };
    }

    private string GetDbName(string dbSuffix)
    {
        return $"coffee-{dbSuffix}";
    }

    private readonly DatabaseService.DatabaseServiceClient databaseService;
}