namespace RandomCoffeeServer.Storage.YandexCloud.Ydb.Helpers;

public static class EnumerableExtensions
{
    private static T? SingleOrNull<T>(this IEnumerable<T> source)
        where T : class
    {
        var foundElement = false;
        T? singleElement = null;
        foreach (var element in source.DefaultIfEmpty())
        {
            if (foundElement)
                return null;
            singleElement = element;
            foundElement = true;
        }

        return singleElement;
    }

    // Пришлось 2 метода, т.к. шарп не может сделать один
    // В одном случае хочется, чтобы он генерил Nullable<Guid> Myfunc(), в другом - User Myfunc(),
    // То есть в зависимости от типа вопросик значит либо другой тип, либо nullable аннотацию
    // Ну и вот он так не может, и, поджав хвост, в таком случае всегда генерит второй вариант - Guid Myfunc(),
    // Оставляя тучу багов в коде!!
    public static TRes? SingleOrNoValue<T, TRes>(this IEnumerable<T> source, Func<T, TRes> resultSelector)
        where T : class
        where TRes : struct
    {
        var element = source.SingleOrDefault();
        return !Equals(element, default(T?)) ? resultSelector(element) : default(TRes?);
    }

    public static TRes? SingleOrNull<T, TRes>(this IEnumerable<T> source, Func<T, TRes> resultSelector)
        where T : class
        where TRes : class
    {
        var element = source.SingleOrDefault();
        return !Equals(element, default(T?)) ? resultSelector(element) : default(TRes?);
    }
}