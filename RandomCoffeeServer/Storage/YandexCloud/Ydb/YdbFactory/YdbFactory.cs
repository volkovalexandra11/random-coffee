using RandomCoffeeServer.Storage.YandexCloud.Lockbox;
using Yandex.Cloud.Credentials;
using Ydb.Sdk.Yc;

namespace RandomCoffeeServer.Storage.YandexCloud.Ydb.YdbFactory;

public class YdbFactory
{
    public YdbFactory(Lazy<LockboxService> lockboxService)
    {
        this.lockboxService = lockboxService;
    }

    public YdbService Create(
        IHostEnvironment environment,
        ILoggerFactory loggerFactory)
    {
        if (Environment.GetEnvironmentVariable("FOLDER_ID") is not { } folderId)
            throw new InvalidProgramException("FOLDER_ID env variable required");

        if (environment.IsProduction())
        {
            var dbDiscoverer = new YdbDiscoverer(new MetadataCredentialsProvider());
            var path = dbDiscoverer.Discover(folderId, ProdDbSuffix).GetAwaiter().GetResult();
            return new YdbService(path, new MetadataProvider(loggerFactory), loggerFactory);
        }
        if (environment.IsDevelopment())
        {
            if (Environment.GetEnvironmentVariable("DEV_DB_PATH") is not { } devDbPath)
                throw new InvalidProgramException("DEV_DB_PATH env variable is required in Development");
            if (Environment.GetEnvironmentVariable("DEV_OAUTH_TOKEN") is not { } devOauthToken)
                throw new InvalidProgramException("DEV_OAUTH_TOKEN env variable is required in Development");

            var dbDiscoverer = new YdbDiscoverer(new OAuthCredentialsProvider(devOauthToken));
            var path = dbDiscoverer.Discover(folderId, dbWithPath: devDbPath).GetAwaiter().GetResult();
            var saKeyPath = GetAndSaveSaKey().GetAwaiter().GetResult();
            return new YdbService(path, new ServiceAccountProvider(saKeyPath), loggerFactory);
        }
        throw new InvalidProgramException("Environment is not one of dev or prod");
    }

    private async Task<string> GetAndSaveSaKey()
    {
        var keyJson = await lockboxService.Value.GetCoffeeDbServiceAccountKey();
        Directory.CreateDirectory(Path.GetDirectoryName(CoffeeDbKeyPath)!);
        File.WriteAllBytesAsync(CoffeeDbKeyPath, keyJson).GetAwaiter().GetResult();
        return CoffeeDbKeyPath;
    }

    private readonly Lazy<LockboxService> lockboxService;

    private const string ProdDbSuffix = "prod";
    private const string CoffeeDbKeyPath = ".keys/coffee-db";
}