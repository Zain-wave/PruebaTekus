namespace PruebaTekus.Application.Common;

public class PagedRequest
{
    private const int MaxPageSize = 100;

    private int _page = 1;
    private int _pageSize = 10;

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Clamp(value, 1, MaxPageSize);
    }

    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool? SortDescending { get; set; }
}