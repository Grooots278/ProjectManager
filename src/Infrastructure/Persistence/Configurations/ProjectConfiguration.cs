using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.CustomerCompany).IsRequired().HasMaxLength(200);
        builder.Property(p => p.ExecutorCompany).IsRequired().HasMaxLength(200);
        builder.Property(p => p.StartDate).IsRequired();
        builder.Property(p => p.EndDate).IsRequired();
        builder.Property(p => p.Priority).IsRequired();

        builder.HasOne(p => p.ProjectManager)
            .WithMany()
            .HasForeignKey(p => p.ProjectManagerId)
            .OnDelete(DeleteBehavior.Restrict); 

        builder.HasMany(p => p.ProjectEmployees)
            .WithOne(pe => pe.Project)
            .HasForeignKey(pe => pe.ProjectId);

        builder.HasIndex(p => p.StartDate);
        builder.HasIndex(p => p.Priority);
    }
}