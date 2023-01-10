namespace RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

public static class EnumerableExtensions
{
    public static T? SingleOrDefault<T>(this IEnumerable<T> source)
    {
        return source.DefaultIfEmpty()
            .Single();
    }

    public static TRes? SingleOrDefault<T, TRes>(this IEnumerable<T> source, Func<T, TRes> resultSelector)
    {
        var element = source.SingleOrDefault();
        return !Equals(element, default(T)) ? resultSelector(element) : default;
    }
}