using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
     public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever(); // Guid generation

        // Convertation Value Objects to owned entities (Owned)
        builder.OwnsOne(e => e.Name, name =>
        {
            name.Property(n => n.FirstName).IsRequired().HasMaxLength(100);
            name.Property(n => n.LastName).IsRequired().HasMaxLength(100);
            name.Property(n => n.MiddleName).HasMaxLength(100);
        });

        builder.OwnsOne(e => e.Email, email =>
        {
            email.Property(e => e.Value).HasColumnName("Email").IsRequired().HasMaxLength(200);
            email.HasIndex(e => e.Value).IsUnique();
        });

        builder.Navigation(e => e.Name).IsRequired();
        builder.Navigation(e => e.Email).IsRequired();
    }
}