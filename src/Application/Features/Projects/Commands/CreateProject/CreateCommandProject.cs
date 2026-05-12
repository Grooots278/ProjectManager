using FluentValidation;
using MediatR;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Features.Projects.Commands.CreateProject;

public record CreateProjectCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
    public string CustomerCompany { get; init; } = string.Empty;
    public string ExecutorCompany { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int Priority { get; init; }
    public Guid ProjectManagerId { get; init; }
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IApplcationDbContext _context;

    public CreateProjectCommandHandler(IApplcationDbContext context) => _context = context;

    public async Task<Guid> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var project = Project.Create(
            command.Name,
            command.CustomerCompany,
            command.ExecutorCompany,
            command.StartDate,
            command.EndDate,
            command.Priority,
            command.ProjectManagerId
        );

        _context.Projects.Add(project);
        await _context.SaveChangeAsync(cancellationToken);
        return project.Id;
    }
}

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CustomerCompany).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ExecutorCompany).NotEmpty().MaximumLength(200);
        RuleFor(x => x.StartDate).LessThan(x => x.EndDate)
            .WithMessage("Start date must be before end date.");
        RuleFor(x => x.Priority).GreaterThan(0);
        RuleFor(x => x.ProjectManagerId).NotEmpty();
    }
}