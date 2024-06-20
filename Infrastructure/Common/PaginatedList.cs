namespace Infrastructure.Common;

public class PaginatedList<T>
{
    public ICollection<T> Items { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}