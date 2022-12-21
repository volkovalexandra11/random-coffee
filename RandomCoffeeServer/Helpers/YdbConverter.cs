using Org.BouncyCastle.Cms;
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

    public static Guid GetGuid(this YdbValue ydbValue)
    {
        return new Guid(ydbValue.GetString());
    }

    public static Guid? GetOptionalGuid(this YdbValue ydbValue)
    {
        var bytes = ydbValue.GetOptionalString();
        return bytes is null ? null : new Guid(bytes);
    }

    public static bool GetBool(this YdbValue ydbValue)
    {
        return ydbValue.GetInt32() != 0;
    }

    public static readonly string DataParamName = "$data";
}