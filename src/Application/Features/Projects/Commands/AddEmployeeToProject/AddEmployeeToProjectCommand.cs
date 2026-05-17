using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManager.Application.Common.Exceptions;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Features.Projects.Commands.AddEmployeeToProject;

public record AddEmployeeToProjectCommand : IRequest
{
    public Guid ProjectId { get; init; }
    public Guid EmployeeId { get; init; }
}

public class AddEmployeeToProjectCommandHandler : IRequestHandler<AddEmployeeToProjectCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<AddEmployeeToProjectCommandHandler> _logger;

    public AddEmployeeToProjectCommandHandler(IApplicationDbContext context, ILogger<AddEmployeeToProjectCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(AddEmployeeToProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync(new object[] { request.ProjectId }, cancellationToken);
        if (project == null)
            throw new NotFoundException(nameof(Project), request.ProjectId);

        var employee = await _context.Employees.FindAsync(new object[] { request.EmployeeId }, cancellationToken);
        if (employee == null)
            throw new NotFoundException(nameof(Employee), request.EmployeeId);

        var alreadyExists = await _context.ProjectEmployees
            .AnyAsync(pe => pe.ProjectId == request.ProjectId && pe.EmployeeId == request.EmployeeId, cancellationToken);
        if (alreadyExists)
        {
            _logger.LogWarning("Employee {EmployeeId} already assigned to project {ProjectId}", request.EmployeeId, request.ProjectId);
            return;
        }

        var projectEmployee = new ProjectEmployee(request.ProjectId, request.EmployeeId);
        _context.ProjectEmployees.Add(projectEmployee);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Employee {EmployeeId} added to project {ProjectId}", request.EmployeeId, request.ProjectId);
    }
}