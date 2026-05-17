using Mapster;
using blazor_project.Data.Repositories.Interfaces;
using blazor_project.Models.Domain;
using blazor_project.Models.DTOs;
using blazor_project.Models.Requests;
using blazor_project.Models.Responses;
using blazor_project.Services.Interfaces;

namespace blazor_project.Services.Customers;

/// <summary>
/// Service implementation for customer-related business operations.
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository repository,
        ILogger<CustomerService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ApiResponse<CustomerDto>> GetCustomerById(int id)
    {
        try
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
            {
                return ApiResponse<CustomerDto>.FailureResponse("Customer not found");
            }

            var dto = customer.Adapt<CustomerDto>();
            return ApiResponse<CustomerDto>.SuccessResponse(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving customer with ID {id}");
            return ApiResponse<CustomerDto>.FailureResponse("An error occurred while retrieving the customer");
        }
    }

    public async Task<PaginatedResponse<CustomerDto>> GetAllCustomers(int pageNumber, int pageSize)
    {
        try
        {
            var (customers, totalCount) = await _repository.GetAllAsync(pageNumber, pageSize);
            var dtos = customers.Adapt<List<CustomerDto>>();

            return new PaginatedResponse<CustomerDto>
            {
                Items = dtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all customers");
            return new PaginatedResponse<CustomerDto>
            {
                Items = new List<CustomerDto>(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = 0
            };
        }
    }

    public async Task<PaginatedResponse<CustomerDto>> SearchCustomers(SearchCriteria criteria)
    {
        try
        {
            if (criteria.PageNumber < 1) criteria.PageNumber = 1;
            if (criteria.PageSize < 1) criteria.PageSize = 10;

            var (customers, totalCount) = await _repository.SearchAsync(criteria);
            var dtos = customers.Adapt<List<CustomerDto>>();

            return new PaginatedResponse<CustomerDto>
            {
                Items = dtos,
                PageNumber = criteria.PageNumber,
                PageSize = criteria.PageSize,
                TotalCount = totalCount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching customers");
            return new PaginatedResponse<CustomerDto>
            {
                Items = new List<CustomerDto>(),
                PageNumber = criteria.PageNumber,
                PageSize = criteria.PageSize,
                TotalCount = 0
            };
        }
    }

    public async Task<ApiResponse<CustomerDto>> CreateCustomer(CreateCustomerRequest request)
    {
        try
        {
            // Check if email already exists
            if (await _repository.EmailExistsAsync(request.Email))
            {
                return ApiResponse<CustomerDto>.FailureResponse(
                    "A customer with this email already exists",
                    new List<string> { "Email is already in use" });
            }

            var customer = request.Adapt<Customer>();
            await _repository.AddAsync(customer);

            var dto = customer.Adapt<CustomerDto>();
            _logger.LogInformation($"Customer created: {customer.CustomerId}");

            return ApiResponse<CustomerDto>.SuccessResponse(dto, "Customer created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer");
            return ApiResponse<CustomerDto>.FailureResponse("An error occurred while creating the customer");
        }
    }

    public async Task<ApiResponse<CustomerDto>> UpdateCustomer(int id, UpdateCustomerRequest request)
    {
        try
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
            {
                return ApiResponse<CustomerDto>.FailureResponse("Customer not found");
            }

            // Check if email is being changed and if it already exists
            if (customer.Email != request.Email && await _repository.EmailExistsAsync(request.Email, id))
            {
                return ApiResponse<CustomerDto>.FailureResponse(
                    "A customer with this email already exists",
                    new List<string> { "Email is already in use" });
            }

            request.Adapt(customer);
            customer.UpdatedDate = DateTime.UtcNow;

            await _repository.UpdateAsync(customer);

            var dto = customer.Adapt<CustomerDto>();
            _logger.LogInformation($"Customer updated: {customer.CustomerId}");

            return ApiResponse<CustomerDto>.SuccessResponse(dto, "Customer updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating customer with ID {id}");
            return ApiResponse<CustomerDto>.FailureResponse("An error occurred while updating the customer");
        }
    }

    public async Task<ApiResponse<bool>> DeleteCustomer(int id)
    {
        try
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null)
            {
                return ApiResponse<bool>.FailureResponse("Customer not found");
            }

            await _repository.DeleteAsync(id);
            _logger.LogInformation($"Customer deleted: {id}");

            return ApiResponse<bool>.SuccessResponse(true, "Customer deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting customer with ID {id}");
            return ApiResponse<bool>.FailureResponse("An error occurred while deleting the customer");
        }
    }
}
