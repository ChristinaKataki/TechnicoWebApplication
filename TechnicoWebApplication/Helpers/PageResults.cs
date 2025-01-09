namespace TechnicoWebApplication.Helpers;
public class PageResults<T>
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<T> Elements { get; set; } = [];
}