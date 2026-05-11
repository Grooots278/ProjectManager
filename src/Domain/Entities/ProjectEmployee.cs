namespace ProjectManager.Domain.Entities;

public class ProjectEmployee
{
    public Guid ProjectId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public Project Project { get; private set; } = null!;
    public Employee Employee { get; private set; } = null!;

    private ProjectEmployee() { } //EF

    public ProjectEmployee(Guid projectId, Guid employeeId)
    {
        ProjectId = projectId;
        EmployeeId = employeeId;
    }
}