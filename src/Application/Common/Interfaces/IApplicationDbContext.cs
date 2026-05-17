using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Project> Projects { get; }
    DbSet<Employee> Employees { get; }
    DbSet<ProjectEmployee> ProjectEmployees { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}