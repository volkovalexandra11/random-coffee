using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

public static class GuidExtensions
{
    public static YdbValue ToYdb(this Guid guid)
    {
        return YdbValue.MakeString(guid.ToByteArray());
    }
}