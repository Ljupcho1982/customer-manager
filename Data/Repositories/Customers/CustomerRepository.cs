using Microsoft.EntityFrameworkCore;
using blazor_project.Models.Domain;
using blazor_project.Models.Requests;
using blazor_project.Data.Repositories.Interfaces;

namespace blazor_project.Data.Repositories.Customers;

/// <summary>
/// Repository implementation for Customer data access operations.
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CustomerId == id && !c.IsDeleted);
    }

    public async Task<(List<Customer> items, int totalCount)> GetAllAsync(int pageNumber, int pageSize)
    {
        var query = _context.Customers
            .Where(c => !c.IsDeleted)
            .OrderByDescending(c => c.CreatedDate);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(List<Customer> items, int totalCount)> SearchAsync(SearchCriteria criteria)
    {
        var query = _context.Customers
            .Where(c => !c.IsDeleted)
            .AsQueryable();

        // Apply search term
        if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
        {
            var searchTerm = criteria.SearchTerm.ToLower();
            query = query.Where(c =>
                c.FirstName.ToLower().Contains(searchTerm) ||
                c.LastName.ToLower().Contains(searchTerm) ||
                c.Email.ToLower().Contains(searchTerm));
        }

        // Apply status filter
        if (!string.IsNullOrWhiteSpace(criteria.Status))
        {
            query = query.Where(c => c.Status == criteria.Status);
        }

        // Apply date range filter
        if (criteria.FromDate.HasValue)
        {
            query = query.Where(c => c.CreatedDate >= criteria.FromDate.Value);
        }

        if (criteria.ToDate.HasValue)
        {
            var toDate = criteria.ToDate.Value.AddDays(1); // Include the entire day
            query = query.Where(c => c.CreatedDate < toDate);
        }

        // Apply city filter
        if (!string.IsNullOrWhiteSpace(criteria.City))
        {
            query = query.Where(c => c.City != null && c.City.ToLower().Contains(criteria.City.ToLower()));
        }

        // Apply country filter
        if (!string.IsNullOrWhiteSpace(criteria.Country))
        {
            query = query.Where(c => c.Country != null && c.Country.ToLower().Contains(criteria.Country.ToLower()));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = criteria.Ascending
            ? query.OrderBy(c => EF.Property<object>(c, criteria.SortBy))
            : query.OrderByDescending(c => EF.Property<object>(c, criteria.SortBy));

        // Apply pagination
        var items = await query
            .Skip((criteria.PageNumber - 1) * criteria.PageSize)
            .Take(criteria.PageSize)
            .AsNoTracking()
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task AddAsync(Customer customer)
    {
        customer.CreatedDate = DateTime.UtcNow;
        customer.UpdatedDate = DateTime.UtcNow;
        
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        var existingCustomer = await _context.Customers.FindAsync(customer.CustomerId);
        if (existingCustomer == null)
        {
            throw new InvalidOperationException($"Customer with ID {customer.CustomerId} not found");
        }

        customer.UpdatedDate = DateTime.UtcNow;
        customer.CreatedDate = existingCustomer.CreatedDate;

        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {id} not found");
        }

        customer.IsDeleted = true;
        customer.UpdatedDate = DateTime.UtcNow;

        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeCustomerId = null)
    {
        var query = _context.Customers
            .Where(c => !c.IsDeleted && c.Email == email);

        if (excludeCustomerId.HasValue)
        {
            query = query.Where(c => c.CustomerId != excludeCustomerId.Value);
        }

        return await query.AnyAsync();
    }
}
