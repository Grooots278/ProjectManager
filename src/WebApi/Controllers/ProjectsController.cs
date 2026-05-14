using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Features.Projects.Commands.CreateProject;
using ProjectManager.Application.Features.Projects.DTOs;
using ProjectManager.Application.Features.Projects.Queries.GetProjects;

namespace ProjectManager.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateProjectCommand command)
    {
        var projectId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = projectId }, projectId);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAll(
        [FromQuery] DateTime? startFrom,
        [FromQuery] DateTime? startTo,
        [FromQuery] int? priority,
        [FromQuery] string? sortBy = "name",
        [FromQuery] bool sortDescending = false
    )
    {
        var query = new GetProjectsQuery
        {
            StartFrom = startFrom,
            StartTo = startTo,
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
        throw new NotImplementedException();
    }
}