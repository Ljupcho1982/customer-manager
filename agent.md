# .NET Customer Management System - Vertical Architecture

## Overview
This document outlines the vertical architecture for a customer management system built with .NET and Blazor, following enterprise best practices and clean architecture principles.

---

## Project Structure

```
blazor_project/
├── Components/
│   ├── Pages/
│   │   ├── Customers/
│   │   │   ├── CustomerList.razor          # Display all customers with search/pagination
│   │   │   ├── CustomerList.razor.cs       # Code-behind logic
│   │   │   ├── CustomerDetail.razor        # Single customer view
│   │   │   ├── CustomerDetail.razor.cs
│   │   │   ├── CustomerForm.razor          # Reusable form for Create/Update
│   │   │   ├── CustomerForm.razor.cs
│   │   │   └── CustomerDelete.razor        # Delete confirmation
│   │   ├── Dashboard.razor                 # Overview/metrics
│   │   └── NotFound.razor
│   ├── Shared/
│   │   ├── SearchBar.razor                 # Reusable search component
│   │   ├── SearchBar.razor.cs
│   │   ├── Pagination.razor                # Reusable pagination
│   │   ├── Pagination.razor.cs
│   │   ├── ConfirmDialog.razor             # Reusable confirmation
│   │   ├── ConfirmDialog.razor.cs
│   │   ├── MainLayout.razor
│   │   └── MainLayout.razor.css
│   ├── _Imports.razor
│   └── App.razor
├── Services/
│   ├── Interfaces/
│   │   ├── ICustomerService.cs             # Customer service contract
│   │   ├── ISearchService.cs               # Search functionality contract
│   │   └── INotificationService.cs         # Toast/notification contract
│   ├── CustomerService.cs                  # Business logic implementation
│   ├── SearchService.cs                    # Search logic
│   └── NotificationService.cs              # UI notifications
├── Models/
│   ├── DTOs/
│   │   ├── CustomerDto.cs                  # Data Transfer Object
│   │   ├── CreateCustomerRequest.cs        # Create request model
│   │   ├── UpdateCustomerRequest.cs        # Update request model
│   │   └── SearchCriteria.cs               # Search parameters
│   ├── Domain/
│   │   └── Customer.cs                     # Domain model
│   └── Responses/
│       ├── ApiResponse.cs                  # Generic response wrapper
│       └── PaginatedResponse.cs            # Paginated results wrapper
├── Data/
│   ├── ApplicationDbContext.cs             # EF Core DbContext
│   ├── Migrations/                         # EF Core migrations
│   └── Repositories/
│       ├── Interfaces/
│       │   └── ICustomerRepository.cs      # Repository contract
│       └── CustomerRepository.cs           # Data access implementation
├── Validators/
│   └── CustomerValidator.cs                # FluentValidation validators
├── Program.cs                              # Dependency injection & startup
├── appsettings.json
├── appsettings.Development.json
├── blazor_project.csproj
└── wwwroot/
    └── css/
        └── custom.css                      # Custom styles
```

---

## Architecture Layers

### 1. **Presentation Layer (UI - Blazor Components)**
- **Purpose**: Handle user interface and user interactions
- **Responsibilities**:
  - Display data to users
  - Collect user input
  - Delegate actions to services
  - Handle UI state and notifications

### 2. **Business Logic Layer (Services)**
- **Purpose**: Implement core business rules
- **Responsibilities**:
  - Validate data
  - Execute business operations
  - Coordinate between presentation and data layers
  - Handle application-level exceptions

### 3. **Data Access Layer (Repositories)**
- **Purpose**: Abstract database operations
- **Responsibilities**:
  - CRUD operations
  - Query execution
  - Data persistence
  - Database connection management

### 4. **Data Layer (Database)**
- **Purpose**: Persist data
- **SQL Server** with Entity Framework Core

---

## Database Schema

### Customer Table
```sql
CREATE TABLE Customers (
    CustomerId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(255),
    City NVARCHAR(100),
    State NVARCHAR(50),
    PostalCode NVARCHAR(20),
    Country NVARCHAR(100),
    Status NVARCHAR(20) NOT NULL DEFAULT 'Active', -- Active/Inactive/Archived
    CreatedDate DATETIME NOT NULL DEFAULT GETUTCDATE(),
    UpdatedDate DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(255),
    UpdatedBy NVARCHAR(255),
    IsDeleted BIT NOT NULL DEFAULT 0
);

CREATE INDEX idx_Email ON Customers(Email);
CREATE INDEX idx_FirstName ON Customers(FirstName);
CREATE INDEX idx_LastName ON Customers(LastName);
CREATE INDEX idx_Status ON Customers(Status);
```

---

## Core Components

### 1. **CustomerList.razor** (Master View)
- Display paginated list of customers
- Integrated search functionality
- Sort by columns
- Action buttons (View, Edit, Delete)
- Bulk operations support

### 2. **CustomerForm.razor** (Create/Update)
- Reusable form component
- Validation with FluentValidation
- Loading states
- Error messages
- Success notifications

### 3. **SearchBar.razor** (Reusable)
- Multi-field search
- Real-time filtering
- Advanced search filters
- Clear/Reset functionality

### 4. **CustomerDetail.razor** (View)
- Read-only customer information
- Related data (if applicable)
- Edit/Delete buttons
- Back navigation

---

## CRUD Operations Implementation

### Create
```
User Input (CustomerForm) 
  ↓
Validation (FluentValidation)
  ↓
Service Layer (CustomerService.Create)
  ↓
Repository Layer (CustomerRepository.Add)
  ↓
Database (EF Core SaveChanges)
  ↓
Response & Notification
```

### Read
```
List View Request
  ↓
Service Layer (CustomerService.GetAll with pagination/search)
  ↓
Repository Layer (CustomerRepository.Query)
  ↓
Database Query
  ↓
DTO Mapping
  ↓
Display in UI
```

### Update
```
Edit Form Load
  ↓
Fetch Customer (Service → Repository)
  ↓
User Modification
  ↓
Validation
  ↓
Service Layer (CustomerService.Update)
  ↓
Repository Layer (CustomerRepository.Update)
  ↓
Database SaveChanges
  ↓
Notification
```

### Delete
```
Delete Action
  ↓
Confirmation Dialog
  ↓
Service Layer (CustomerService.Delete)
  ↓
Repository Layer (Soft Delete or Hard Delete)
  ↓
Database Update
  ↓
List Refresh
```

---

## Search Implementation

### SearchService Features
- **Full-text search** across FirstName, LastName, Email
- **Advanced filters**: Status, DateRange, City, Country
- **Pagination**: Page size, current page
- **Sorting**: By column and direction
- **Performance**: Query optimization with indexes

### Search Bar Component
- Debounced input
- Multiple filter options
- Visual feedback during search
- Clear filters button
- Recent searches (optional)

---

## Best Practices Implemented

### 1. **Architecture & Design Patterns**
- ✅ Vertical Slice Architecture (feature-based organization)
- ✅ Repository Pattern (data abstraction)
- ✅ Dependency Injection (loose coupling)
- ✅ SOLID Principles
- ✅ Clean Architecture
- ✅ DTOs for data transfer

### 2. **Code Quality**
- ✅ Separation of concerns (layers)
- ✅ Code-behind for complex Blazor components
- ✅ Interface-based design
- ✅ Async/await patterns
- ✅ Error handling and logging
- ✅ Input validation (server & client-side)

### 3. **Data Access**
- ✅ Entity Framework Core for ORM
- ✅ Async operations (async/await)
- ✅ Database indexes for search performance
- ✅ Soft delete support
- ✅ Audit fields (CreatedDate, UpdatedDate, CreatedBy, UpdatedBy)
- ✅ Query optimization

### 4. **Blazor Component Best Practices**
- ✅ Component composition (reusable components)
- ✅ Parameter binding
- ✅ Event callbacks
- ✅ Lifecycle hooks (OnInitializedAsync)
- ✅ State management
- ✅ CSS isolation
- ✅ Form validation

### 5. **Security**
- ✅ Input validation and sanitization
- ✅ SQL injection prevention (EF Core parameterized queries)
- ✅ XSS prevention (Blazor built-in)
- ✅ Authentication/Authorization (add as needed)
- ✅ Data protection for sensitive fields

### 6. **Performance**
- ✅ Pagination for large datasets
- ✅ Lazy loading components
- ✅ Debounced search
- ✅ Database indexes
- ✅ Async operations to prevent blocking
- ✅ CSS/JS isolation

### 7. **User Experience**
- ✅ Loading states and spinners
- ✅ Success/error notifications
- ✅ Confirmation dialogs for destructive actions
- ✅ Form validation feedback
- ✅ Responsive design
- ✅ Accessibility considerations

### 8. **Maintenance & Testing**
- ✅ Unit testable services
- ✅ Mockable repositories
- ✅ Logging for debugging
- ✅ Configuration management
- ✅ Documentation
- ✅ Consistent naming conventions

---

## Key Files Implementation Guide

### ICustomerService.cs
```csharp
public interface ICustomerService
{
    Task<ApiResponse<CustomerDto>> GetCustomerById(int id);
    Task<PaginatedResponse<CustomerDto>> GetAllCustomers(SearchCriteria criteria);
    Task<ApiResponse<CustomerDto>> CreateCustomer(CreateCustomerRequest request);
    Task<ApiResponse<CustomerDto>> UpdateCustomer(int id, UpdateCustomerRequest request);
    Task<ApiResponse<bool>> DeleteCustomer(int id);
    Task<PaginatedResponse<CustomerDto>> SearchCustomers(SearchCriteria criteria);
}
```

### ICustomerRepository.cs
```csharp
public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(int id);
    Task<List<Customer>> GetAllAsync(int pageNumber, int pageSize);
    Task<(List<Customer> items, int totalCount)> SearchAsync(
        string searchTerm, 
        int pageNumber, 
        int pageSize,
        string sortBy = "CreatedDate",
        bool ascending = false);
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(int id);
}
```

### Dependency Injection (Program.cs)
```csharp
// Add services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// Add EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Validators
builder.Services.AddScoped<CustomerValidator>();
```

---

## Development Workflow

### 1. **Feature Development**
- Create feature folder (e.g., `/Components/Pages/Customers/`)
- Add Page component (.razor)
- Add Code-behind (.razor.cs)
- Add supporting components (Form, Detail, etc.)
- Create/Update Service
- Create/Update Repository
- Add validation
- Add tests

### 2. **Database Changes**
```bash
# Create migration
dotnet ef migrations add AddCustomerTable

# Update database
dotnet ef database update
```

### 3. **Running the Application**
```bash
dotnet run
```

---

## Testing Strategy

### Unit Tests
- Service layer logic
- Validation rules
- Repository queries

### Integration Tests
- Database operations
- Service-repository interaction
- API endpoints

### Component Tests
- Blazor component behavior
- Event handling
- Parameter binding

---

## Future Enhancements

- [ ] Authentication & Authorization
- [ ] Audit logging
- [ ] Export to Excel/PDF
- [ ] Bulk import from CSV
- [ ] Advanced reporting
- [ ] Email notifications
- [ ] File attachments
- [ ] Multi-tenancy support
- [ ] API documentation (Swagger)
- [ ] Background jobs (Hangfire)
- [ ] Caching strategy
- [ ] Real-time updates (SignalR)

---

## Configuration & Deployment

### Connection String (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=CustomerManagement;Trusted_Connection=true;"
  }
}
```

### Deployment Considerations
- Entity Framework migrations in CI/CD pipeline
- Database backups
- SSL/TLS for production
- Environment-specific configurations
- Logging and monitoring setup

---

## References & Resources

- [Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [FluentValidation](https://fluentvalidation.net/)
- [Repository Pattern](https://martinfowler.com/eaaCatalog/repository.html)

---

**Last Updated**: May 17, 2026
**Architecture Version**: 1.0
