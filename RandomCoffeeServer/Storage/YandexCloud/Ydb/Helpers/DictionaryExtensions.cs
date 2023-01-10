using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

public static class DictionaryExtensions
{
    public static Dictionary<string, YdbValue> ToDataParams(this Dictionary<string, YdbValue> data)
    {
        const string dataParamName = "$data";
        return new Dictionary<string, YdbValue>
        {
            [dataParamName] = YdbValue.MakeList(new[]
            {
                YdbValue.MakeStruct(data)
            })
        };
    }
}