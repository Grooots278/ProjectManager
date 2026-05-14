using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Features.Projects.Commands.CreateProject;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Persistence;

namespace ProjectManager.Application.UnitTests.Projects;

public class CreateProjectTests
{
    [Fact]
     public async Task Handle_ValidCommand_ShouldCreateProjectAndReturnId()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using var context = new AppDbContext(options);
        // Adding manager
        var manager = Employee.Create(
            Domain.ValueObjects.FullName.Create("Иван", "Иванов"),
            Domain.ValueObjects.Email.Create("ivan@example.com"));
        context.Employees.Add(manager);
        await context.SaveChangesAsync();

        var handler = new CreateProjectCommandHandler(context);

        var command = new CreateProjectCommand
        {
            Name = "Test Project",
            CustomerCompany = "Customer Inc",
            ExecutorCompany = "Executor LLC",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30),
            Priority = 1,
            ProjectManagerId = manager.Id
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        var project = await context.Projects.FindAsync(result);
        Assert.NotNull(project);
        Assert.Equal("Test Project", project.Name);
    }

}