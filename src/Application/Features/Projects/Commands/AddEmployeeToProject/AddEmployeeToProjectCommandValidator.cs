using FluentValidation;

namespace ProjectManager.Application.Features.Projects.Commands.AddEmployeeToProject;

public class AddEmployeeToProjectCommandValidator : AbstractValidator<AddEmployeeToProjectCommand>
{
    public AddEmployeeToProjectCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
    }
}