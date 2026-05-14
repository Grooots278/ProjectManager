namespace ProjectManager.Domain.Entities;

public class Project
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string CustomerCompany { get; private set; }
    public string ExecutorCompany { get; private set; }
    public Guid ProjectManagerId { get; private set; } // Supervisor (one of the employees)
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int Priority { get; private set; }

    //Navigation properties
    public Employee ProjectManager { get; private set; } = null!;
    private readonly List<ProjectEmployee> _projectEmployees = new();
    public IReadOnlyCollection<ProjectEmployee> ProjectEmployees => _projectEmployees.AsReadOnly();

    private Project() { } //EF

    private Project(string name, string customerCompany, string executorCompany,
        DateTime startDate, DateTime endDate, int priority, Guid projectManagerId)
    {
        Id = Guid.NewGuid();
        Name = name;
        CustomerCompany = customerCompany;
        ExecutorCompany = executorCompany;
        StartDate = startDate;
        EndDate = endDate;
        Priority = priority;
        ProjectManagerId = projectManagerId;
    }

    public static Project Create(string name, string customerCompany, string executorCompany, 
    DateTime startDate, DateTime endDate, int priority, Guid projectManagerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Project name is required", nameof(name));
        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date");

        return new Project(name, customerCompany, executorCompany, startDate, endDate, priority, projectManagerId);
    }

    public void Update(string name, string customerCompany, string executorCompany,
    DateTime startDate, DateTime endDate, int priority, Guid projectManagerId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Project name is required", nameof(name));
        if (endDate <= startDate)
            throw new ArgumentException("End date must be after start date");

        Name = name;
        CustomerCompany = customerCompany;
        ExecutorCompany = executorCompany;
        StartDate = startDate;
        EndDate = endDate;
        Priority = priority;
        ProjectManagerId = projectManagerId;
    }
}