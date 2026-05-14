using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;

namespace ProjectManager.Application.Features.Employees.Queries.GetEmployees;

public record GetEmployeesQuery : IRequest<List<EmployeeDto>>
{
    public string? NameFilter { get; init; }
    public string? EmailFilter { get; init; }
    public string? SortBy { get; init; } 
    public bool SortDescending { get; init; }
}

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, List<EmployeeDto>>
{
    private readonly IApplcationDbContext _context;
    private readonly IMapper _mapper;

    public GetEmployeesQueryHandler(IApplcationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.NameFilter))
        {
            var filter = request.NameFilter.Trim().ToLower();
            query = query.Where(e => e.Name.FirstName.ToLower().Contains(filter)
                                     || e.Name.LastName.ToLower().Contains(filter)
                                     || (e.Name.MiddleName != null && e.Name.MiddleName.ToLower().Contains(filter)));
        }

        if (!string.IsNullOrWhiteSpace(request.EmailFilter))
        {
            var emailFilter = request.EmailFilter.Trim().ToLower();
            query = query.Where(e => e.Email.Value.Contains(emailFilter));
        }

        query = request.SortBy?.ToLower() switch
        {
            "name" => request.SortDescending
                ? query.OrderByDescending(e => e.Name.LastName).ThenByDescending(e => e.Name.FirstName)
                : query.OrderBy(e => e.Name.LastName).ThenBy(e => e.Name.FirstName),
            "email" => request.SortDescending
                ? query.OrderByDescending(e => e.Email.Value)
                : query.OrderBy(e => e.Email.Value),
            _ => query.OrderBy(e => e.Name.LastName)
        };

        var employees = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<EmployeeDto>>(employees);
    }
}