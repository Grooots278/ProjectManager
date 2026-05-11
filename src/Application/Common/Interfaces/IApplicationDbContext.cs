using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Common.Interfaces;

public interface IApplcationDbContext
{
    DbSet<Project> Projects { get; }
    DbSet<Employee> Employees { get; }
    DbSet<ProjectEmployee> ProjectEmployees { get; }

    Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
}