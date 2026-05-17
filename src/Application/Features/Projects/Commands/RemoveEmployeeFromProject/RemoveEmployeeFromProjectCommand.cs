using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectManager.Application.Common.Exceptions;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Features.Projects.Commands.RemoveEmployeeFromProject;

public record RemoveEmployeeFromProjectCommand : IRequest
{
    public Guid ProjectId { get; init; }
    public Guid EmployeeId { get; init; }
}

public class RemoveEmployeeFromProjectCommandHandler : IRequestHandler<RemoveEmployeeFromProjectCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<RemoveEmployeeFromProjectCommandHandler> _logger;

    public RemoveEmployeeFromProjectCommandHandler(IApplicationDbContext context, ILogger<RemoveEmployeeFromProjectCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(RemoveEmployeeFromProjectCommand request, CancellationToken cancellationToken)
    {
        var projectEmployee = await _context.ProjectEmployees
            .FirstOrDefaultAsync(pe => pe.ProjectId == request.ProjectId && pe.EmployeeId == request.EmployeeId, cancellationToken);

        if (projectEmployee == null)
        {
            _logger.LogWarning("Assignment Project {ProjectId} / Employee {EmployeeId} not found", request.ProjectId, request.EmployeeId);
            throw new NotFoundException(nameof(ProjectEmployee), $"{request.ProjectId}-{request.EmployeeId}");
        }

        _context.ProjectEmployees.Remove(projectEmployee);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Employee {EmployeeId} removed from project {ProjectId}", request.EmployeeId, request.ProjectId);
    }
}