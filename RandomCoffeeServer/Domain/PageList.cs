using System.Web;
using Microsoft.AspNetCore.Http.Extensions;

namespace RandomCoffeeServer.Domain;

//public class PageList<T> : List<T>
public class PageList<T>
{
    public int Page { get; set; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public long TotalCount { get; }
    public IEnumerable<T> Items { get; set; }
    public Uri? NextPage { get; private set; }

    private PageList(HttpRequest request, IEnumerable<T> items, long totalCount, int page, int pageSize)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        Page = page;
        TotalPages = (int) Math.Ceiling(totalCount / (double) pageSize);
        Items = items;
        SetNextPage(request);
    }

    public static PageList<T> ToPageList(HttpRequest request, IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PageList<T>(request, items, count, pageNumber, pageSize);
    }

    private void SetNextPage(HttpRequest request)
    {
        var uriBuilder = new UriBuilder(request.GetEncodedUrl());
        var queryString = HttpUtility.ParseQueryString(uriBuilder.Query);
        queryString.Set(nameof(Page).ToLower(), (Page + 1).ToString());
        uriBuilder.Query = queryString.ToString();
        NextPage = uriBuilder.Uri;
    }
}