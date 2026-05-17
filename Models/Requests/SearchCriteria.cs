namespace blazor_project.Models.Requests;

/// <summary>
/// Search and filtering criteria for customer queries.
/// </summary>
public class SearchCriteria
{
    public string? SearchTerm { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string SortBy { get; set; } = "CreatedDate";

    public bool Ascending { get; set; } = false;

    // Advanced filters
    public string? Status { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }
}
