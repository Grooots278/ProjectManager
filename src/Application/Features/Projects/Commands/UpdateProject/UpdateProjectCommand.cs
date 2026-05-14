using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ProjectManager.Application.Common.Exceptions;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string CustomerCompany { get; init; } = string.Empty;
    public string ExecutorCompany { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int Priority { get; init; }
    public Guid ProjectManagerId { get; init; }
}

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand>
{
    private readonly IApplcationDbContext _context;
    private readonly ILogger<UpdateProjectCommandHandler> _logger;

    public UpdateProjectCommandHandler(IApplcationDbContext context, ILogger<UpdateProjectCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects.FindAsync(new object[] { request.Id }, cancellationToken);
        if (project == null)
        {
            _logger.LogWarning("Project with ID {ProjectId} not found for update", request.Id);
            throw new NotFoundException(nameof(Project), request.Id);
        }

        project.Update(
            request.Name,
            request.CustomerCompany,
            request.ExecutorCompany,
            request.StartDate,
            request.EndDate,
            request.Priority,
            request.ProjectManagerId);

        await _context.SaveChangeAsync(cancellationToken);
        _logger.LogInformation("Project {ProjectId} updated successfully", project.Id);
    }
}

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CustomerCompany).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ExecutorCompany).NotEmpty().MaximumLength(200);
        RuleFor(x => x.StartDate).LessThan(x => x.EndDate)
            .WithMessage("Start date must be before end date.");
        RuleFor(x => x.Priority).GreaterThan(0);
        RuleFor(x => x.ProjectManagerId).NotEmpty();
    }
}