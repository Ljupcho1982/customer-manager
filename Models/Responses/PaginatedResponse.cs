namespace blazor_project.Models.Responses;

/// <summary>
/// Paginated response wrapper for list results.
/// </summary>
/// <typeparam name="T">The type of items in the list</typeparam>
public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = new();

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }

    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}
