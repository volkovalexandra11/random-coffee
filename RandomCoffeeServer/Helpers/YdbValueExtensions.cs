using Ydb.Sdk.Value;

namespace RandomCoffeeServer.Helpers;

public static class YdbValueExtensions
{
    // custom
    public static Guid GetNonNullGuid(this YdbValue ydbValue)
    {
        var bytes = ydbValue.GetOptionalString();
        if (bytes is null)
            throw new InvalidProgramException();
        return new Guid(bytes);
    }
    
    public static bool GetNonNullBool(this YdbValue ydbValue)
    {
        var @bool = ydbValue.GetOptionalInt32();
        if (@bool is null)
            throw new InvalidProgramException();
        return @bool.Value != 0;
    }

    // built-in
    public static int GetNonNullInt32(this YdbValue ydbValue)
    {
        var int32 = ydbValue.GetOptionalInt32();
        if (int32 is null)
            throw new InvalidProgramException();
        return int32.Value;
    }

    public static uint GetNonNullUint32(this YdbValue ydbValue)
    {
        var uint32 = ydbValue.GetOptionalUint32();
        if (uint32 is null)
            throw new InvalidProgramException();
        return uint32.Value;
    }

    public static long GetNonNullInt64(this YdbValue ydbValue)
    {
        var int64 = ydbValue.GetOptionalInt64();
        if (int64 is null)
            throw new InvalidProgramException();
        return int64.Value;
    }

    public static ulong GetNonNullUint64(this YdbValue ydbValue)
    {
        var uint64 = ydbValue.GetOptionalUint64();
        if (uint64 is null)
            throw new InvalidProgramException();
        return uint64.Value;
    }

    public static byte GetNonNullUint8(this YdbValue ydbValue)
    {
        var uint8 = ydbValue.GetOptionalUint8();
        if (uint8 is null)
            throw new InvalidProgramException();
        return uint8.Value;
    }

    public static sbyte GetNonNullInt8(this YdbValue ydbValue)
    {
        var int8 = ydbValue.GetOptionalInt8();
        if (int8 is null)
            throw new InvalidProgramException();
        return int8.Value;
    }

    public static short GetNonNullInt16(this YdbValue ydbValue)
    {
        var int16 = ydbValue.GetOptionalInt16();
        if (int16 is null)
            throw new InvalidProgramException();
        return int16.Value;
    }

    public static ushort GetNonNullUint16(this YdbValue ydbValue)
    {
        var uint16 = ydbValue.GetOptionalUint16();
        if (uint16 is null)
            throw new InvalidProgramException();
        return uint16.Value;
    }

    public static double GetNonNullDouble(this YdbValue ydbValue)
    {
        var @double = ydbValue.GetOptionalDouble();
        if (@double is null)
            throw new InvalidProgramException();
        return @double.Value;
    }

    public static float GetNonNullFloat(this YdbValue ydbValue)
    {
        var @float = ydbValue.GetOptionalFloat();
        if (@float is null)
            throw new InvalidProgramException();
        return @float.Value;
    }

    public static DateTime GetNonNullDate(this YdbValue ydbValue)
    {
        var date = ydbValue.GetOptionalDate();
        if (date is null)
            throw new InvalidProgramException();
        return date.Value;
    }

    public static DateTime GetNonNullDatetime(this YdbValue ydbValue)
    {
        var datetime = ydbValue.GetOptionalDatetime();
        if (datetime is null)
            throw new InvalidProgramException();
        return datetime.Value;
    }

    public static DateTime GetNonNullTimestamp(this YdbValue ydbValue)
    {
        var timestamp = ydbValue.GetOptionalTimestamp();
        if (timestamp is null)
            throw new InvalidProgramException();
        return timestamp.Value;
    }

    public static TimeSpan GetNonNullInterval(this YdbValue ydbValue)
    {
        var interval = ydbValue.GetOptionalInterval();
        if (interval is null)
            throw new InvalidProgramException();
        return interval.Value;
    }

    public static byte[] GetNonNullString(this YdbValue ydbValue)
    {
        var @string = ydbValue.GetOptionalString();
        if (@string is null)
            throw new InvalidProgramException();
        return @string;
    }

    public static string GetNonNullUtf8(this YdbValue ydbValue)
    {
        var utf8 = ydbValue.GetOptionalUtf8();
        if (utf8 is null)
            throw new InvalidProgramException();
        return utf8;
    }

    public static byte[] GetNonNullYson(this YdbValue ydbValue)
    {
        var yson = ydbValue.GetOptionalYson();
        if (yson is null)
            throw new InvalidProgramException();
        return yson;
    }

    public static string GetNonNullJson(this YdbValue ydbValue)
    {
        var json = ydbValue.GetOptionalJson();
        if (json is null)
            throw new InvalidProgramException();
        return json;
    }

    public static string GetNonNullJsonDocument(this YdbValue ydbValue)
    {
        var jsonDocument = ydbValue.GetOptionalJsonDocument();
        if (jsonDocument is null)
            throw new InvalidProgramException();
        return jsonDocument;
    }
}