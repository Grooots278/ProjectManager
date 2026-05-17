using MediatR;
using Microsoft.Extensions.Logging;
using ProjectManager.Application.Common.Exceptions;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Features.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(Guid Id) : IRequest;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DeleteProjectCommandHandler> _logger;

    public DeleteProjectCommandHandler(IApplicationDbContext context, ILogger<DeleteProjectCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync(new object[] { request.Id }, cancellationToken);
        if (project == null)
        {
            _logger.LogWarning("Project with ID {ProjectId} not found for deletion", request.Id);
            throw new NotFoundException(nameof(Project), request.Id);
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Project {ProjectId} deleted", request.Id);
    }
}