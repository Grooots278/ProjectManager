using MediatR;
using Microsoft.Extensions.Logging;
using ProjectManager.Application.Common.Exceptions;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.ValueObjects;

namespace ProjectManager.Application.Features.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand : IRequest
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string? MiddleName { get; init; }
    public string Email { get; init; } = string.Empty;
}

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<UpdateEmployeeCommandHandler> _logger;

    public UpdateEmployeeCommandHandler(IApplicationDbContext context, ILogger<UpdateEmployeeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FindAsync(new object[] { request.Id }, cancellationToken);
        if (employee == null)
        {
            _logger.LogWarning("Employee with ID {EmployeeId} not found for update", request.Id);
            throw new NotFoundException(nameof(Employee), request.Id);
        }

        var newName = FullName.Create(request.FirstName, request.LastName, request.MiddleName);
        var newEmail = Email.Create(request.Email);

        employee.Update(newName, newEmail);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Employee {EmployeeId} updated", employee.Id);
    }
}