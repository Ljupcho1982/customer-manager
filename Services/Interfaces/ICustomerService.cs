using blazor_project.Models.DTOs;
using blazor_project.Models.Requests;
using blazor_project.Models.Responses;

namespace blazor_project.Services.Interfaces;

/// <summary>
/// Service interface for customer-related business operations.
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Gets a customer by ID.
    /// </summary>
    Task<ApiResponse<CustomerDto>> GetCustomerById(int id);

    /// <summary>
    /// Gets all customers with pagination.
    /// </summary>
    Task<PaginatedResponse<CustomerDto>> GetAllCustomers(int pageNumber, int pageSize);

    /// <summary>
    /// Searches for customers based on criteria.
    /// </summary>
    Task<PaginatedResponse<CustomerDto>> SearchCustomers(SearchCriteria criteria);

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    Task<ApiResponse<CustomerDto>> CreateCustomer(CreateCustomerRequest request);

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    Task<ApiResponse<CustomerDto>> UpdateCustomer(int id, UpdateCustomerRequest request);

    /// <summary>
    /// Deletes a customer.
    /// </summary>
    Task<ApiResponse<bool>> DeleteCustomer(int id);
}
