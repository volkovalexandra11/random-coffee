using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

public static class StringExtensions
{
    public static YdbValue ToYdb(this string str)
    {
        return YdbValue.MakeUtf8(str);
    }
}