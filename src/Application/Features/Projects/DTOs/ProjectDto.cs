namespace ProjectManager.Application.Features.Projects.DTOs;

public record ProjectDto(
    Guid Id,
    string Name,
    string CustomerCompany,
    string ExecutorCompany,
    Guid ProjectManagerId,
    string ProjectManagerFullName,
    DateTime StartDate,
    DateTime EndDate,
    int Priority,
    List<string> EmployeeEmails,
    List<Guid> EmployeeIds
);