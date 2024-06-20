namespace Application.Common;

public class Pagination<T> where T : class
{
    public List<T> Items { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPage { get; set; }

    public static Pagination<T> Create(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = source.Count();
        var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        var totalPage = (int)Math.Ceiling(count / (double)pageSize);

        return new Pagination<T>()
        {
            Items = items.ToList(),
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalPage = totalPage
        };
    }
}
