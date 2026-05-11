using ProjectManager.Domain.ValueObjects;

namespace ProjectManager.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public FullName Name { get; private set; }
    public Email Email { get; private set; }

    // Navigation properties to EF
    private readonly List<ProjectEmployee> _projectEmployees = new();
    private IReadOnlyCollection<ProjectEmployee> ProjectEmployees => _projectEmployees.AsReadOnly();

    private Employee() { } //EF

    private Employee(FullName fullName, Email email)
    {
        Id = Guid.NewGuid();
        Name = fullName;
        Email = email;
    }

    public static Employee Create(FullName fullName, Email email)
    {
        return new Employee(fullName, email);
    }
}