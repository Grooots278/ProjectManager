namespace ProjectManager.Application.Features.Employees.Queries.GetEmployees;

public record EmployeeDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? MiddleName,
    string Email
);