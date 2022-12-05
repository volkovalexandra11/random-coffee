using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Helpers;

public static class YdbConverter
{
    public static Dictionary<string, YdbValue> ToDataParams(Dictionary<string, YdbValue> data)
    {
        return new Dictionary<string, YdbValue>
        {
            [DataParamName] = YdbValue.MakeList(new[]
            {
                YdbValue.MakeStruct(data)
            })
        };
    }

    public static readonly string DataParamName = "$data";
}