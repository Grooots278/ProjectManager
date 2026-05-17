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
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetEmployeesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Employees.AsQueryable();

        // 1. Email filtering — you can leave it in the database (it works)
        if (!string.IsNullOrWhiteSpace(request.EmailFilter))
        {
            var emailFilter = request.EmailFilter.Trim().ToLowerInvariant();
            query = query.Where(e => e.Email.Value.ToLowerInvariant().Contains(emailFilter));
        }

        // 2. Sorting is also performed at the database level (optional if the volume is large)
        query = request.SortBy?.ToLowerInvariant() switch
        {
            "name" => request.SortDescending
                ? query.OrderByDescending(e => e.Name.LastName).ThenByDescending(e => e.Name.FirstName)
                : query.OrderBy(e => e.Name.LastName).ThenBy(e => e.Name.FirstName),
            "email" => request.SortDescending
                ? query.OrderByDescending(e => e.Email.Value)
                : query.OrderBy(e => e.Email.Value),
            _ => query.OrderBy(e => e.Name.LastName)
        };

        // 3. We get employees from the database (without the name filter)
        var employees = await query.ToListAsync(cancellationToken);

        // 4. Filtering by name — now in memory, works with any encoding
        if (!string.IsNullOrWhiteSpace(request.NameFilter))
        {
            var filter = request.NameFilter.Trim().ToLowerInvariant();
                employees = employees.Where(e =>
                    e.Name.FirstName.ToLowerInvariant().Contains(filter) ||
                    e.Name.LastName.ToLowerInvariant().Contains(filter) ||
                    (e.Name.MiddleName != null && e.Name.MiddleName.ToLowerInvariant().Contains(filter))
                ).ToList();
        }

        return _mapper.Map<List<EmployeeDto>>(employees);
    }
}