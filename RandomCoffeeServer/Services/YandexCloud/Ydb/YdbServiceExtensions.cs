using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.OpenApi.Extensions;
using RandomCoffee.QueryBuilder;
using RandomCoffee.schema;
using Ydb.Sdk.Client;
using Ydb.Sdk.Value;

namespace RandomCoffee;

// не нужен
public class YdbTypeAttribute : Attribute
{
    public YdbTypeAttribute(YdbTypeId ydbTypeId)
    {
        YdbTypeId = ydbTypeId;
    }

    public YdbTypeId YdbTypeId { get; }
}

public static class YdbServiceExtensions
{
    public static IResponse? Replace<T>(YdbTable table, T value)
    {
        var properties = GetProperties<T>();
        var propertiesDict = properties
            .ToDictionary(
                property => CaseConverter.CamelToSnake(property.Name),
                property => GetYdbValue(value, property)
            ); //new Dictionary<string, YdbValue>()
        // var data = YdbValueConverter.ToDataParams(propertiesDict);
        return null;
    }

    private static YdbValue GetYdbValue<T>(T value, PropertyInfo property)
    {
        var propertyValue = property.GetValue(value);
        if (propertyValue is null)
        {
            throw new Exception();
        }

        var type = property.GetCustomAttribute<YdbTypeAttribute>()?.YdbTypeId;
        if (type is null)
        {
            return YdbValueConverter.ToYdbValue(propertyValue);
        }
        return YdbValueConverter.ToYdbValue(propertyValue, type.Value);
    }

    private static PropertyInfo[] GetProperties<T>()
    {
        var type = typeof(T);
        if (!propertyCache.TryGetValue(type, out var properties))
        {
            properties = type.GetProperties();
            propertyCache[type] = properties;
        }
        return properties;
    }

    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> propertyCache = new();
}