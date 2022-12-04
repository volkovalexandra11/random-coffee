using Ydb.Sdk.Value;

namespace RandomCoffee.QueryBuilder;

public class YdbValueConverter
{
    // generated with regex on all available Ydb types
    // no Date/DateTime bc of ambiguousity
    // bool -> Int32 because why the hell doesn't sdk have bool
    public static YdbValue ToYdbValue(object obj)
    {
        // 
        // todo: weird bool & date

        if (obj is int @Int32)
            return YdbValue.MakeInt32(@Int32);
        if (obj is uint @Uint32)
            return YdbValue.MakeUint32(@Uint32);
        if (obj is long @Int64)
            return YdbValue.MakeInt64(@Int64);
        if (obj is ulong @Uint64)
            return YdbValue.MakeUint64(@Uint64);
        if (obj is byte @Uint8)
            return YdbValue.MakeUint8(@Uint8);
        if (obj is bool @Bool)
            return YdbValue.MakeInt32(@Bool ? 1 : 0); // that's cringe
        if (obj is sbyte @Int8)
            return YdbValue.MakeInt8(@Int8);
        if (obj is short @Int16)
            return YdbValue.MakeInt16(@Int16);
        if (obj is ushort @Uint16)
            return YdbValue.MakeUint16(@Uint16);
        if (obj is double @Double)
            return YdbValue.MakeDouble(@Double);
        if (obj is float @Float)
            return YdbValue.MakeFloat(@Float);
        if (obj is TimeSpan @Interval)
            return YdbValue.MakeInterval(@Interval);
        if (obj is byte[] @String)
            return YdbValue.MakeString(@String);
        if (obj is string @Utf8)
            return YdbValue.MakeUtf8(@Utf8);
        if (obj is IReadOnlyList<YdbValue> @ListType)
            return YdbValue.MakeList(@ListType);
        if (obj is IReadOnlyDictionary<string, YdbValue> @StructType)
            return YdbValue.MakeStruct(@StructType);

        throw new ArgumentException(
            $"Can't convert type {obj.GetType()} to {nameof(YdbValue)}");
    }

    // public static YdbValue ToYdbValue(int value) { return YdbValue.MakeInt32(value); }
    // public static YdbValue ToYdbValue(uint value) { return YdbValue.MakeUint32(value); }
    // public static YdbValue ToYdbValue(long value) { return YdbValue.MakeInt64(value); }
    // public static YdbValue ToYdbValue(ulong value) { return YdbValue.MakeUint64(value); }
    // public static YdbValue ToYdbValue(byte value) { return YdbValue.MakeUint8(value); }
    // public static YdbValue ToYdbValue(bool value) { return YdbValue.MakeInt32(value ? 0 : 1); }
    // public static YdbValue ToYdbValue(sbyte value) { return YdbValue.MakeInt8(value); }
    // public static YdbValue ToYdbValue(short value) { return YdbValue.MakeInt16(value); }
    // public static YdbValue ToYdbValue(ushort value) { return YdbValue.MakeUint16(value); }
    // public static YdbValue ToYdbValue(double value) { return YdbValue.MakeDouble(value); }
    // public static YdbValue ToYdbValue(float value) { return YdbValue.MakeFloat(value); }
    // public static YdbValue ToYdbValue(TimeSpan value) { return YdbValue.MakeInterval(value); }

    public static YdbValue ToYdbValue<T>(T value)
    {
        return value switch
        {
            int @Int32 => YdbValue.MakeInt32(@Int32),
            uint @Uint32 => YdbValue.MakeUint32(@Uint32),
            long @Int64 => YdbValue.MakeInt64(@Int64),
            ulong @Uint64 => YdbValue.MakeUint64(@Uint64),
            byte @Uint8 => YdbValue.MakeUint8(@Uint8),
            bool @Bool => YdbValue.MakeInt32(@Bool ? 1 : 0),
            sbyte @Int8 => YdbValue.MakeInt8(@Int8),
            short @Int16 => YdbValue.MakeInt16(@Int16),
            ushort @Uint16 => YdbValue.MakeUint16(@Uint16),
            double @Double => YdbValue.MakeDouble(@Double),
            float @Float => YdbValue.MakeFloat(@Float),
            TimeSpan @Interval => YdbValue.MakeInterval(@Interval),
            byte[] @String => YdbValue.MakeString(@String),
            string @Utf8 => YdbValue.MakeUtf8(@Utf8),
            IReadOnlyList<YdbValue> @ListType => YdbValue.MakeList(@ListType),
            IReadOnlyDictionary<string, YdbValue> @StructType => YdbValue.MakeStruct(@StructType),
            _ => throw new ArgumentException($"Can't convert type {value.GetType()} to {nameof(YdbValue)}")
        };
    }

    public static YdbValue ToYdbValue(object obj, YdbTypeId typeId)
    {
        // 
        // todo: weird bool & date

        if (obj is int @Int32)
            return YdbValue.MakeInt32(@Int32);
        if (obj is uint @Uint32)
            return YdbValue.MakeUint32(@Uint32);
        if (obj is long @Int64)
            return YdbValue.MakeInt64(@Int64);
        if (obj is ulong @Uint64)
            return YdbValue.MakeUint64(@Uint64);
        if (obj is byte @Uint8)
            return YdbValue.MakeUint8(@Uint8);
        if (obj is bool @Bool)
            return YdbValue.MakeInt32(@Bool ? 1 : 0); // that's cringe
        if (obj is sbyte @Int8)
            return YdbValue.MakeInt8(@Int8);
        if (obj is short @Int16)
            return YdbValue.MakeInt16(@Int16);
        if (obj is ushort @Uint16)
            return YdbValue.MakeUint16(@Uint16);
        if (obj is double @Double)
            return YdbValue.MakeDouble(@Double);
        if (obj is float @Float)
            return YdbValue.MakeFloat(@Float);
        if (obj is TimeSpan @Interval)
            return YdbValue.MakeInterval(@Interval);
        if (obj is byte[] @String)
            return YdbValue.MakeString(@String);
        if (obj is string @Utf8)
            return YdbValue.MakeUtf8(@Utf8);
        if (obj is IReadOnlyList<YdbValue> @ListType)
            return YdbValue.MakeList(@ListType);
        if (obj is IReadOnlyDictionary<string, YdbValue> @StructType)
            return YdbValue.MakeStruct(@StructType);

        throw new ArgumentException(
            $"Can't convert type {obj.GetType()} to {nameof(YdbValue)}");
    }

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