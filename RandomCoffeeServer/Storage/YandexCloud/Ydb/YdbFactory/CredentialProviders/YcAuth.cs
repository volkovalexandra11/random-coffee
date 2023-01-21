namespace RandomCoffeeServer.Storage.YandexCloud.Ydb.YdbFactory;

// is depencency of ServiceAccountYdb.cs
// https://github.com/ydb-platform/ydb-dotnet-yc/blob/main/src/Ydb.Sdk.Yc.Auth/src/YcAuth.cs
static class YcAuth
{
    public static readonly string DefaultAudience = "https://iam.api.cloud.yandex.net/iam/v1/tokens";
    public static readonly string DefaultEndpoint = "iam.api.cloud.yandex.net:443";

    public static readonly string MetadataUrl =
        "http://169.254.169.254/computeMetadata/v1/instance/service-accounts/default/token";
};

class EmptyYcCredentialsProvider : Yandex.Cloud.Credentials.ICredentialsProvider
{
    public string GetToken()
    {
        return "";
    }
}