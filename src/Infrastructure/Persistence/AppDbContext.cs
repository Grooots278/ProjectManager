using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplcationDbContext
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<ProjectEmployee> ProjectEmployees => Set<ProjectEmployee>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public Task<int> SaveChangeAsync(CancellationToken cancellationToken)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}