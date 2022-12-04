using Ydb.Sdk.Auth;
using Ydb.Sdk.Yc;

namespace RandomCoffee.CredentialProviders;

public static class CredentialProviderFactory
{
    public static ICredentialsProvider GetAnonymous()
    {
        return new AnonymousProvider();
    }

    public static ICredentialsProvider GetOAuth()
    {
        return new TokenProvider("мой токен");
    }

    public static async Task<ICredentialsProvider> GetSa()
    {
        var p = new ServiceAccountProvider("keys/ydb.jwt");
        await p.Initialize();
        return p;
    }

    public static async Task<ICredentialsProvider> GetMetadata()
    {
        var p = new MetadataProvider();
        await p.Initialize();
        return p;
    }
}