using MediatR;
using Microsoft.Extensions.Logging;
using ProjectManager.Application.Common.Exceptions;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Features.Employees.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(Guid Id) : IRequest;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DeleteEmployeeCommandHandler> _logger;

    public DeleteEmployeeCommandHandler(IApplicationDbContext context, ILogger<DeleteEmployeeCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FindAsync(new object[] { request.Id }, cancellationToken);
        if (employee == null)
        {
            _logger.LogWarning("Employee with ID {EmployeeId} not found for deletion", request.Id);
            throw new NotFoundException(nameof(Employee), request.Id);
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Employee {EmployeeId} deleted", request.Id);
    }
}
