using blazor_project.Models.Domain;
using FluentValidation;

namespace blazor_project.Validators;

/// <summary>
/// FluentValidation validator for Customer entity.
/// </summary>
public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(100)
            .WithMessage("First name cannot exceed 100 characters");

        RuleFor(c => c.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(100)
            .WithMessage("Last name cannot exceed 100 characters");

        RuleFor(c => c.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format")
            .MaximumLength(255)
            .WithMessage("Email cannot exceed 255 characters");

        RuleFor(c => c.PhoneNumber)
            .MaximumLength(20)
            .WithMessage("Phone number cannot exceed 20 characters")
            .When(c => !string.IsNullOrEmpty(c.PhoneNumber));

        RuleFor(c => c.Address)
            .MaximumLength(255)
            .WithMessage("Address cannot exceed 255 characters")
            .When(c => !string.IsNullOrEmpty(c.Address));

        RuleFor(c => c.City)
            .MaximumLength(100)
            .WithMessage("City cannot exceed 100 characters")
            .When(c => !string.IsNullOrEmpty(c.City));

        RuleFor(c => c.State)
            .MaximumLength(50)
            .WithMessage("State cannot exceed 50 characters")
            .When(c => !string.IsNullOrEmpty(c.State));

        RuleFor(c => c.PostalCode)
            .MaximumLength(20)
            .WithMessage("Postal code cannot exceed 20 characters")
            .When(c => !string.IsNullOrEmpty(c.PostalCode));

        RuleFor(c => c.Country)
            .MaximumLength(100)
            .WithMessage("Country cannot exceed 100 characters")
            .When(c => !string.IsNullOrEmpty(c.Country));

        RuleFor(c => c.Status)
            .NotEmpty()
            .WithMessage("Status is required")
            .Must(s => new[] { "Active", "Inactive", "Archived" }.Contains(s))
            .WithMessage("Status must be Active, Inactive, or Archived");
    }
}
