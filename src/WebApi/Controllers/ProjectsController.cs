using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Features.Projects.Commands.AddEmployeeToProject;
using ProjectManager.Application.Features.Projects.Commands.CreateProject;
using ProjectManager.Application.Features.Projects.Commands.DeleteProject;
using ProjectManager.Application.Features.Projects.Commands.RemoveEmployeeFromProject;
using ProjectManager.Application.Features.Projects.Commands.UpdateProject;
using ProjectManager.Application.Features.Projects.DTOs;
using ProjectManager.Application.Features.Projects.Queries.GetProjectById;
using ProjectManager.Application.Features.Projects.Queries.GetProjects;

namespace ProjectManager.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAll(
        [FromQuery] string? nameFilter,
        [FromQuery] string? customerFilter,
        [FromQuery] string? executorFilter,
        [FromQuery] DateTime? startFrom,
        [FromQuery] DateTime? startTo,
        [FromQuery] DateTime? endFrom,
        [FromQuery] DateTime? endTo,
        [FromQuery] int? priority,
        [FromQuery] string? sortBy = "name",
        [FromQuery] bool sortDescending = false)
    {
        var query = new GetProjectsQuery
        {
            NameFilter = nameFilter,
            CustomerFilter = customerFilter,
            ExecutorFilter = executorFilter,
            StartFrom = startFrom,
            StartTo = startTo,
            EndFrom = endFrom,
            EndTo = endTo,
            Priority = priority,
            SortBy = sortBy,
            SortDescending = sortDescending
        };
        var projects = await _mediator.Send(query);
        return Ok(projects);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectDto>> GetById(Guid id)
    {
        var project = await _mediator.Send(new GetProjectByIdQuery(id));
        return project is not null ? Ok(project) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateProjectCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID in URL does not match ID in body");

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteProjectCommand(id));
        return NoContent();
    }

    [HttpPost("{projectId:guid}/employees/{employeeId:guid}")]
    public async Task<IActionResult> AddEmployee(Guid projectId, Guid employeeId)
    {
        await _mediator.Send(new AddEmployeeToProjectCommand { ProjectId = projectId, EmployeeId = employeeId });
        return NoContent();
    }

    [HttpDelete("{projectId:guid}/employees/{employeeId:guid}")]
    public async Task<IActionResult> RemoveEmployee(Guid projectId, Guid employeeId)
    {
        await _mediator.Send(new RemoveEmployeeFromProjectCommand { ProjectId = projectId, EmployeeId = employeeId });
        return NoContent();
    }
}