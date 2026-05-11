using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

public class ProjectEmployeeConfiguration : IEntityTypeConfiguration<ProjectEmployee>
{
    public void Configure(EntityTypeBuilder<ProjectEmployee> builder)
    {
        builder.HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

        builder.HasOne(pe => pe.Project)
            .WithMany(p => p.ProjectEmployees)
            .HasForeignKey(pe => pe.ProjectId);

        builder.HasOne(pe => pe.Employee)
            .WithMany(e => e.ProjectEmployees)
            .HasForeignKey(pe => pe.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}