using Ydb.Sdk.Value;

namespace RandomCoffee.QueryBuilder;

public static class YdbTypes
{
    public static YdbTypeId ToYdbTypeId(Type type)
    {
        if (type == typeof(int)) return YdbTypeId.Int32;
        if (type == typeof(uint)) return YdbTypeId.Uint32;
        if (type == typeof(long)) return YdbTypeId.Int64;
        if (type == typeof(ulong)) return YdbTypeId.Uint64;
        if (type == typeof(byte)) return YdbTypeId.Uint8;
        if (type == typeof(bool)) return YdbTypeId.Bool;
        if (type == typeof(sbyte)) return YdbTypeId.Int8;
        if (type == typeof(short)) return YdbTypeId.Int16;
        if (type == typeof(ushort)) return YdbTypeId.Uint16;
        if (type == typeof(double)) return YdbTypeId.Double;
        if (type == typeof(float)) return YdbTypeId.Float;
        if (type == typeof(TimeSpan)) return YdbTypeId.Interval;
        if (type == typeof(byte[])) return YdbTypeId.String;
        if (type == typeof(string)) return YdbTypeId.Utf8;
        if (type == typeof(IReadOnlyList<YdbValue>)) return YdbTypeId.ListType;
        if (type == typeof(IReadOnlyDictionary<string, YdbValue>)) return YdbTypeId.StructType;
        throw new ArgumentException();
    }
}