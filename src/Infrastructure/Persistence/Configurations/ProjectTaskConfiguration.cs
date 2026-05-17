using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Persistence.Configurations;

public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();
        builder.Property(t => t.Name).IsRequired().HasMaxLength(300);
        builder.Property(t => t.Priority).IsRequired();
        builder.Property(t => t.Status)
            .HasConversion<int>()
            .IsRequired();
        builder.Property(t => t.Comment).HasMaxLength(2000);
        builder.Property(t => t.CreatedAt).IsRequired();

        builder.HasOne(t => t.Project)
            .WithMany() 
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade); 

        builder.HasOne(t => t.Author)
            .WithMany()
            .HasForeignKey(t => t.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Assignee)
            .WithMany()
            .HasForeignKey(t => t.AssigneeId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.Priority);
        builder.HasIndex(t => t.ProjectId);
    }
}