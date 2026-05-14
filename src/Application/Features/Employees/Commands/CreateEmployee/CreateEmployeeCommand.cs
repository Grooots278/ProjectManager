using MediatR;
using Microsoft.Extensions.Logging;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.ValueObjects;

namespace ProjectManager.Application.Features.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand : IRequest<Guid>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? MiddleName { get; init; }
    public string Email { get; init; } = string.Empty;
}

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Guid>
{
    private readonly IApplcationDbContext _context;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(IApplcationDbContext context, ILogger<CreateEmployeeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var fullName = FullName.Create(command.FirstName, command.LastName, command.MiddleName);
        var email = Email.Create(command.Email);

        var employee = Employee.Create(fullName, email);

        _context.Employees.Add(employee);
        await _context.SaveChangeAsync(cancellationToken);

        _logger.LogInformation("Employee {EmployeeId} created with email {Email}", employee.Id, email);
        return employee.Id;
    }
}