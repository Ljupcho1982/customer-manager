using blazor_project.Models.Domain;
using blazor_project.Models.Requests;

namespace blazor_project.Data.Repositories.Interfaces;

/// <summary>
/// Repository interface for Customer data access operations.
/// </summary>
public interface ICustomerRepository
{
    /// <summary>
    /// Gets a customer by their ID.
    /// </summary>
    Task<Customer?> GetByIdAsync(int id);

    /// <summary>
    /// Gets all customers with pagination.
    /// </summary>
    Task<(List<Customer> items, int totalCount)> GetAllAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Searches for customers based on criteria.
    /// </summary>
    Task<(List<Customer> items, int totalCount)> SearchAsync(SearchCriteria criteria);

    /// <summary>
    /// Adds a new customer to the database.
    /// </summary>
    Task AddAsync(Customer customer);

    /// <summary>
    /// Updates an existing customer in the database.
    /// </summary>
    Task UpdateAsync(Customer customer);

    /// <summary>
    /// Deletes a customer (soft delete).
    /// </summary>
    Task DeleteAsync(int id);

    /// <summary>
    /// Checks if a customer with the given email exists.
    /// </summary>
    Task<bool> EmailExistsAsync(string email, int? excludeCustomerId = null);
}
