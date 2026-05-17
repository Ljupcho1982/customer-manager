namespace blazor_project.Models.Domain;

/// <summary>
/// Customer domain model representing a customer entity in the system.
/// </summary>
public class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string Status { get; set; } = "Active"; // Active, Inactive, Archived

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets the full name of the customer.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();
}
