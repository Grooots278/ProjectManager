using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.Features.Projects.DTOs;

namespace ProjectManager.Application.Features.Projects.Queries.GetProjects;

public record GetProjectsQuery : IRequest<List<ProjectDto>>
{
    public DateTime? StartFrom { get; init; }
    public DateTime? StartTo { get; init; }
    public int? Priority { get; init; }
    public string? SortBy { get; init; } // for example "name", "startdate", "priority"
    public bool SortDescending { get; init; }
}

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, List<ProjectDto>>
{
    private readonly IApplcationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectsQueryHandler(IApplcationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Projects
            .Include(p => p.ProjectManager)
            .Include(p => p.ProjectEmployees)
                .ThenInclude(pe => pe.Employee)
            .AsQueryable();

        // Filtration
        if (request.StartFrom.HasValue)
            query = query.Where(p => p.StartDate >= request.StartFrom.Value);
        if (request.StartTo.HasValue)
            query = query.Where(p => p.StartDate <= request.StartTo.Value);
        if (request.Priority.HasValue)
            query = query.Where(p => p.Priority == request.Priority.Value);

        query = request.SortBy?.ToLower() switch
        {
            "name" => request.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "startdate" => request.SortDescending ? query.OrderByDescending(p => p.StartDate) : query.OrderBy(p => p.StartDate),
            "priority" => request.SortDescending ? query.OrderByDescending(p => p.Priority) : query.OrderBy(p => p.Priority),
            _ => query.OrderBy(p => p.Name) // default
        };

        var projects = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<ProjectDto>>(projects);
    }
}