using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using blazor_project.Models.Domain;

namespace blazor_project.Data;

/// <summary>
/// Entity Framework Core database context for the application.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20);

            entity.Property(e => e.Address)
                .HasMaxLength(255);

            entity.Property(e => e.City)
                .HasMaxLength(100);

            entity.Property(e => e.State)
                .HasMaxLength(50);

            entity.Property(e => e.PostalCode)
                .HasMaxLength(20);

            entity.Property(e => e.Country)
                .HasMaxLength(100);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Active");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.FirstName);
            entity.HasIndex(e => e.LastName);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.City);
            entity.HasIndex(e => e.IsDeleted);
        });
    }
}
