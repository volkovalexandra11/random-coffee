namespace RandomCoffeeServer.Domain.Services.Coffee.UserMatching;

public static class IListExtensions
{
    // https://stackoverflow.com/questions/273313/randomize-a-listt
    public static IList<T> Shuffled<T>(this IList<T> list, Random random)
    {
        var shuffled = list.ToList();

        var swapNoFurtherThan = list.Count;
        while (swapNoFurtherThan > 1)
        {
            swapNoFurtherThan--;

            var swapWith = random.Next(swapNoFurtherThan + 1);
            (shuffled[swapWith], shuffled[swapNoFurtherThan]) =
                (shuffled[swapNoFurtherThan], shuffled[swapWith]);
        }

        return shuffled;
    }
}