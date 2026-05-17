using ProjectManager.Domain.Enums;

namespace ProjectManager.Domain.Entities;

public class ProjectTask
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid AuthorId { get; private set; }
    public Guid? AssigneeId { get; private set; }
    public TaskStatuses Status { get; private set; }
    public string? Comment { get; private set; }
    public int Priority { get; private set; }
    public DateTime CreatedAt { get; private set; }

    //Communication with the project (required)
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;

    //Navigation properties for employees
    public Employee Author { get; private set; } = null!;
    public Employee? Assignee { get; private set; }

    private ProjectTask() { }

    private ProjectTask(
        string name,
        Guid authorId,
        int priority,
        Guid projectId,
        string? comment = null,
        Guid? assigneeId = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        AuthorId = authorId;
        Priority = priority;
        ProjectId = projectId;
        Comment = comment;
        AssigneeId = assigneeId;
        Status = TaskStatuses.ToDo;
        CreatedAt = DateTime.UtcNow;
    }

    public static ProjectTask Create(
        string name,
        Guid authorId,
        int priority,
        Guid projectId,
        string? comment = null,
        Guid? assigneeId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Task name is required", nameof(name));
        if (priority <= 0)
            throw new ArgumentException("Priority must be positive", nameof(priority));

        return new ProjectTask(name, authorId, priority, projectId, comment, assigneeId);
    }

    public void Update(
        string name,
        int priority,
        Guid? assigneeId,
        TaskStatuses status,
        string? comment)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Task name is required", nameof(name));
        if (priority <= 0)
            throw new ArgumentException("Priority must be positive", nameof(priority));

        Name = name;
        Priority = priority;
        AssigneeId = assigneeId;
        Status = status;
        Comment = comment;
    }

    public void ChangeStatus(TaskStatuses newStatus) => Status = newStatus;

    public void AssignEmployee(Guid employeeId) => AssigneeId = employeeId;

    public void UnassignEmployee() => AssigneeId = null;
}