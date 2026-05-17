# Vertical Slice Architecture Skill Guide

## Overview
This skill guide provides comprehensive documentation for implementing a vertical slice architecture in a .NET Blazor customer management system. Vertical slice architecture organizes features by business capability rather than technical layers.

---

## What is Vertical Slice Architecture?

### Traditional Layered Approach (Horizontal)
```
┌─────────────────────────────────┐
│   UI Layer (Controllers/Views)  │
├─────────────────────────────────┤
│   Business Logic Layer          │
├─────────────────────────────────┤
│   Data Access Layer             │
├─────────────────────────────────┤
│   Database                      │
└─────────────────────────────────┘
```

### Vertical Slice Architecture
```
┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐
│   Customers     │  │   Orders        │  │   Reports       │
├─────────────────┤  ├─────────────────┤  ├─────────────────┤
│ - Components    │  │ - Components    │  │ - Components    │
│ - Services      │  │ - Services      │  │ - Services      │
│ - Repositories  │  │ - Repositories  │  │ - Repositories  │
│ - Models        │  │ - Models        │  │ - Models        │
│ - Validators    │  │ - Validators    │  │ - Validators    │
└─────────────────┘  └─────────────────┘  └─────────────────┘
```

---

## Key Principles

### 1. **Feature Ownership**
- Each vertical slice is self-contained
- Minimal dependencies between slices
- Clear responsibility boundaries
- Easy to understand and maintain

### 2. **Scalability**
- Add new features without touching existing code
- Team can work on multiple features independently
- Easier to scale horizontally
- Reduced merge conflicts

### 3. **Maintainability**
- All code for a feature in one location
- Easy to locate and modify feature code
- Simple to add tests
- Reduced cognitive load

### 4. **Decoupling**
- Shared utilities in common folders
- Interfaces for cross-slice communication
- Dependency Injection for loose coupling
- Minimal shared state

---

## Project Organization

### Root-Level Structure
```
blazor_project/
├── Components/              # Blazor UI
│   ├── Pages/
│   │   └── Customers/      # Feature Slice
│   ├── Shared/             # Shared components
│   └── _Imports.razor
├── Services/               # Business logic (by feature)
│   ├── Customers/
│   └── Common/
├── Data/                   # Data access
│   ├── Repositories/
│   └── Migrations/
├── Models/                 # Domain & DTOs
│   ├── Customers/
│   └── Common/
├── Validators/             # Validation rules
│   └── Customers/
├── Program.cs
└── appsettings.json
```

### Customers Feature Slice (Example)
```
Customers/
├── CustomerList.razor
├── CustomerList.razor.cs
├── CustomerDetail.razor
├── CustomerDetail.razor.cs
├── CustomerForm.razor
├── CustomerForm.razor.cs
├── CustomerDelete.razor
├── CustomerDelete.razor.cs
├── Services/
│   └── ICustomerService.cs
│   └── CustomerService.cs
├── Models/
│   ├── CustomerDto.cs
│   ├── CreateCustomerRequest.cs
│   └── UpdateCustomerRequest.cs
├── Validators/
│   └── CustomerValidator.cs
└── Repositories/
    ├── ICustomerRepository.cs
    └── CustomerRepository.cs
```

---

## Shared Components & Services

### Shared Utilities (Reusable Across Slices)
```
Shared/
├── Components/
│   ├── SearchBar.razor
│   ├── Pagination.razor
│   ├── ConfirmDialog.razor
│   ├── DataTable.razor
│   ├── Loading.razor
│   └── NoData.razor
├── Services/
│   ├── INotificationService.cs
│   ├── NotificationService.cs
│   ├── IAuthService.cs
│   └── AuthService.cs
├── Models/
│   ├── ApiResponse.cs
│   ├── PaginatedResponse.cs
│   ├── SearchCriteria.cs
│   └── Constants.cs
└── Extensions/
    ├── ServiceCollectionExtensions.cs
    └── QueryableExtensions.cs
```

---

## Dependency Injection & Service Registration

### Program.cs - Feature Registration
```csharp
// Customers Feature
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<CustomerValidator>();

// Shared Services
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Common
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper for DTO mapping
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
```

---

## CRUD Operations Flow (Detailed)

### Create Operation
```
1. User navigates to Create page
   └─> CustomerForm.razor (Create mode)

2. Form loads and displays empty fields
   └─> OnInitializedAsync()
   └─> Initialize form state

3. User fills form and submits
   └─> OnValidSubmit() in CustomerForm.razor.cs

4. Validation occurs
   └─> ClientValidator (Blazor built-in)
   └─> FluentValidation rules

5. Service processes request
   └─> CustomerService.CreateAsync()
   └─> Apply business rules
   └─> Audit trail

6. Repository saves to database
   └─> CustomerRepository.AddAsync()
   └─> EF Core tracking
   └─> SaveChangesAsync()

7. Response handling
   └─> Success: Show notification, redirect
   └─> Failure: Show error, maintain form state

8. UI updates
   └─> Navigate to list or detail page
```

### Read Operation (List with Search)
```
1. User opens Customer List page
   └─> CustomerList.razor

2. Page initializes with default parameters
   └─> OnInitializedAsync()
   └─> SearchCriteria (page=1, pageSize=10)

3. Fetch data from service
   └─> CustomerService.SearchAsync()
   └─> Apply filters
   └─> Pagination

4. Query database
   └─> CustomerRepository.SearchAsync()
   └─> Execute filtered query
   └─> Count total records

5. Map to DTOs
   └─> Customer entity → CustomerDto
   └─> Exclude sensitive data
   └─> Format for display

6. Render UI
   └─> Table with customer data
   └─> Pagination controls
   └─> Search/filter options
```

### Update Operation
```
1. User clicks Edit on customer
   └─> Navigate to CustomerForm (Edit mode)
   └─> Pass customer ID as parameter

2. Form loads with customer data
   └─> OnInitializedAsync()
   └─> Fetch customer: CustomerService.GetByIdAsync()
   └─> Populate form fields

3. User modifies data and submits
   └─> OnValidSubmit()

4. Validation occurs (same as Create)

5. Service updates customer
   └─> CustomerService.UpdateAsync()
   └─> Apply business rules
   └─> Update audit fields

6. Repository updates database
   └─> CustomerRepository.UpdateAsync()
   └─> EF Core change tracking
   └─> SaveChangesAsync()

7. Response & Navigation
   └─> Success: Redirect to detail/list
   └─> Failure: Show error, keep form
```

### Delete Operation
```
1. User clicks Delete on customer
   └─> Show confirmation dialog
   └─> ConfirmDialog component

2. User confirms deletion
   └─> Call CustomerService.DeleteAsync()

3. Service processes deletion
   └─> Validate customer exists
   └─> Check dependencies
   └─> Soft delete or hard delete

4. Repository executes delete
   └─> CustomerRepository.DeleteAsync()
   └─> Update IsDeleted flag (soft delete)
   └─> SaveChangesAsync()

5. Response handling
   └─> Success: Remove from list, show notification
   └─> Failure: Show error message

6. UI updates
   └─> Refresh customer list
   └─> Navigate if deleting current record
```

---

## Search Implementation Details

### SearchCriteria Model
```csharp
public class SearchCriteria
{
    public string SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = "CreatedDate";
    public bool Ascending { get; set; } = false;
    
    // Advanced filters
    public string Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}
```

### Search Service Logic
```csharp
public async Task<PaginatedResponse<CustomerDto>> SearchAsync(SearchCriteria criteria)
{
    var query = _context.Customers.AsQueryable();

    // Apply search term
    if (!string.IsNullOrEmpty(criteria.SearchTerm))
    {
        var term = criteria.SearchTerm.ToLower();
        query = query.Where(c => 
            c.FirstName.ToLower().Contains(term) ||
            c.LastName.ToLower().Contains(term) ||
            c.Email.ToLower().Contains(term));
    }

    // Apply filters
    if (!string.IsNullOrEmpty(criteria.Status))
        query = query.Where(c => c.Status == criteria.Status);

    if (criteria.FromDate.HasValue)
        query = query.Where(c => c.CreatedDate >= criteria.FromDate);

    if (criteria.ToDate.HasValue)
        query = query.Where(c => c.CreatedDate <= criteria.ToDate);

    // Apply sorting
    query = criteria.Ascending 
        ? query.OrderBy(c => EF.Property<object>(c, criteria.SortBy))
        : query.OrderByDescending(c => EF.Property<object>(c, criteria.SortBy));

    // Pagination
    var total = await query.CountAsync();
    var items = await query
        .Skip((criteria.PageNumber - 1) * criteria.PageSize)
        .Take(criteria.PageSize)
        .ToListAsync();

    return new PaginatedResponse<CustomerDto>
    {
        Items = _mapper.Map<List<CustomerDto>>(items),
        TotalCount = total,
        PageNumber = criteria.PageNumber,
        PageSize = criteria.PageSize
    };
}
```

---

## Component Patterns

### Parent Component Pattern
```csharp
// CustomerList.razor.cs
@page "/customers"
@inject ICustomerService CustomerService
@inject NavigationManager Navigation

public List<CustomerDto> Customers { get; set; } = new();
public SearchCriteria CurrentSearch { get; set; } = new();

protected override async Task OnInitializedAsync()
{
    await LoadCustomers();
}

private async Task LoadCustomers()
{
    var response = await CustomerService.SearchAsync(CurrentSearch);
    Customers = response.Items;
}

private void OnSearch(SearchCriteria criteria)
{
    CurrentSearch = criteria;
    CurrentSearch.PageNumber = 1; // Reset to first page
}

private void OnPageChanged(int pageNumber)
{
    CurrentSearch.PageNumber = pageNumber;
}
```

### Child Component Pattern (Reusable Form)
```csharp
// CustomerForm.razor.cs
@inject ICustomerService CustomerService
@inject INotificationService Notification

[Parameter]
public int? CustomerId { get; set; }

[Parameter]
public EventCallback OnSuccess { get; set; }

private CreateCustomerRequest Model = new();
private bool IsLoading = false;

protected override async Task OnInitializedAsync()
{
    if (CustomerId.HasValue)
    {
        var customer = await CustomerService.GetCustomerById(CustomerId.Value);
        Model = _mapper.Map<CreateCustomerRequest>(customer.Data);
    }
}

private async Task HandleSubmit()
{
    IsLoading = true;
    try
    {
        var response = CustomerId.HasValue
            ? await CustomerService.UpdateCustomer(CustomerId.Value, Model)
            : await CustomerService.CreateCustomer(Model);

        if (response.Success)
        {
            Notification.ShowSuccess("Operation completed successfully");
            await OnSuccess.InvokeAsync();
        }
        else
        {
            Notification.ShowError(response.Message);
        }
    }
    finally
    {
        IsLoading = false;
    }
}
```

---

## Error Handling Strategy

### Global Exception Handling
```csharp
// In Program.cs
app.UseExceptionHandler("/error");

// In ErrorHandler service
public void HandleException(Exception ex)
{
    if (ex is ValidationException validationEx)
    {
        // Handle validation errors
        NotifyValidationError(validationEx.Errors);
    }
    else if (ex is NotFoundException notFoundEx)
    {
        // Handle not found
        Navigate("404");
    }
    else if (ex is UnauthorizedException)
    {
        // Handle unauthorized
        Navigate("/login");
    }
    else
    {
        // Handle general errors
        Logger.LogError(ex, "Unhandled exception");
        NotifyError("An unexpected error occurred");
    }
}
```

---

## Testing Strategy

### Unit Testing Services
```csharp
[TestClass]
public class CustomerServiceTests
{
    private Mock<ICustomerRepository> _mockRepository;
    private CustomerService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ICustomerRepository>();
        _service = new CustomerService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task CreateCustomer_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var request = new CreateCustomerRequest { Email = "test@example.com" };
        
        // Act
        var result = await _service.CreateCustomer(request);
        
        // Assert
        Assert.IsTrue(result.Success);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Customer>()), Times.Once);
    }
}
```

### Component Testing
```csharp
[TestClass]
public class CustomerListTests
{
    private Mock<ICustomerService> _mockService;
    private Fixture _fixture;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<ICustomerService>();
        _fixture = new Fixture();
    }

    [TestMethod]
    public async Task OnInitializedAsync_LoadsCustomers()
    {
        // Arrange
        var customers = _fixture.CreateMany<CustomerDto>(5).ToList();
        _mockService.Setup(s => s.SearchAsync(It.IsAny<SearchCriteria>()))
            .ReturnsAsync(new PaginatedResponse<CustomerDto> { Items = customers });

        // Act & Assert
        // Component loads and displays customers
    }
}
```

---

## Performance Optimization

### 1. Database Level
- Create indexes on frequently searched columns
- Use projections to select only needed fields
- Implement query pagination
- Use AsNoTracking() for read-only operations

### 2. Application Level
- Implement caching (in-memory, Redis)
- Use lazy loading for related data
- Debounce search input
- Implement pagination

### 3. UI Level
- Virtual scrolling for large lists
- Lazy load components
- CSS/JS isolation
- Minimize re-renders

---

## Best Practices Summary

| Category | Practice |
|----------|----------|
| **Organization** | One feature per slice, shared code in common folders |
| **Dependencies** | Use interfaces, inject via DI, minimize cross-slice coupling |
| **Data** | DTOs for transfers, entities for database, validators for rules |
| **Async** | Use async/await everywhere, avoid blocking calls |
| **Validation** | Dual-layer (client & server), FluentValidation for complex rules |
| **Errors** | Global exception handling, meaningful error messages |
| **Testing** | Unit test services, integration test repositories, component test UI |
| **Security** | Validate all input, protect sensitive data, use authorization |
| **Performance** | Pagination, indexing, caching, query optimization |
| **UI/UX** | Loading states, confirmations, notifications, responsive design |

---

## Common Pitfalls to Avoid

❌ **Don't** - Create tight coupling between slices  
✅ **Do** - Use interfaces and dependency injection

❌ **Don't** - Store state in components  
✅ **Do** - Use services for state management

❌ **Don't** - Skip validation  
✅ **Do** - Validate on client and server

❌ **Don't** - Make blocking database calls  
✅ **Do** - Use async/await throughout

❌ **Don't** - Mix business logic with UI  
✅ **Do** - Keep them separate in services

---

## Feature Development Checklist

- [ ] Create feature folder structure
- [ ] Define domain model
- [ ] Create DTOs for data transfer
- [ ] Implement validators
- [ ] Create repository interface and implementation
- [ ] Create service interface and implementation
- [ ] Register services in DI container
- [ ] Create Blazor page component
- [ ] Create form/detail components
- [ ] Add validation to components
- [ ] Implement search functionality
- [ ] Add error handling
- [ ] Create unit tests
- [ ] Create integration tests
- [ ] Update database migration
- [ ] Test manually
- [ ] Document changes

---

## References

- Vertical Slice Architecture: https://jimmybogard.com/vertical-slice-architecture/
- Clean Architecture: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html
- SOLID Principles: https://www.digitalocean.com/community/conceptual_articles/s-o-l-i-d-the-first-five-principles-of-object-oriented-design
- Blazor Best Practices: https://learn.microsoft.com/en-us/aspnet/core/blazor/best-practices
- Entity Framework Core: https://learn.microsoft.com/en-us/ef/core/
- FluentValidation: https://fluentvalidation.net/

---

**Version**: 1.0  
**Last Updated**: May 17, 2026
