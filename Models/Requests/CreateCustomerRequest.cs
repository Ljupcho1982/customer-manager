using System.ComponentModel.DataAnnotations;

namespace blazor_project.Models.Requests;

/// <summary>
/// Request model for creating a new customer.
/// </summary>
public class CreateCustomerRequest
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number")]
    public string? PhoneNumber { get; set; }

    [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
    public string? Address { get; set; }

    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public string? City { get; set; }

    [StringLength(50, ErrorMessage = "State cannot exceed 50 characters")]
    public string? State { get; set; }

    [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters")]
    public string? PostalCode { get; set; }

    [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
    public string? Country { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = "Active";
}
