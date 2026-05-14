using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Features.Employees.Commands.CreateEmployee;
using ProjectManager.Application.Features.Employees.Commands.DeleteEmployee;
using ProjectManager.Application.Features.Employees.Commands.UpdateEmployee;
using ProjectManager.Application.Features.Employees.Queries.GetEmployeeById;
using ProjectManager.Application.Features.Employees.Queries.GetEmployees;

namespace ProjectManager.WebApi.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> GetAll(
        [FromQuery] string? nameFilter,
        [FromQuery] string? emailFilter,
        [FromQuery] string? sortBy = "name",
        [FromQuery] bool sortDescending = false)
    {
        var query = new GetEmployeesQuery
        {
            NameFilter = nameFilter,
            EmailFilter = emailFilter,
            SortBy = sortBy,
            SortDescending = sortDescending
        };
        var employees = await _mediator.Send(query);
        return Ok(employees);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployeeDto>> GetById(Guid id)
    {
        var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));
        return employee is not null ? Ok(employee) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateEmployeeCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteEmployeeCommand(id));
        return NoContent();
    }
}