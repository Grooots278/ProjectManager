using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.Features.Projects.DTOs;

namespace ProjectManager.Application.Features.Projects.Queries.GetProjects;

public record GetProjectsQuery : IRequest<List<ProjectDto>>
{
    public string? NameFilter { get; init; }
    public string? CustomerFilter { get; init; }
    public string? ExecutorFilter { get; init; }
    public DateTime? StartFrom { get; init; }
    public DateTime? StartTo { get; init; }
    public DateTime? EndFrom { get; init; }
    public DateTime? EndTo { get; init; }
    public int? Priority { get; init; }
    public string? SortBy { get; init; }
    public bool SortDescending { get; init; }
}

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, List<ProjectDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectsQueryHandler(IApplicationDbContext context, IMapper mapper)
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

        if (!string.IsNullOrWhiteSpace(request.NameFilter))
            query = query.Where(p => p.Name.Contains(request.NameFilter));
        if (!string.IsNullOrWhiteSpace(request.CustomerFilter))
            query = query.Where(p => p.CustomerCompany.Contains(request.CustomerFilter));
        if (!string.IsNullOrWhiteSpace(request.ExecutorFilter))
            query = query.Where(p => p.ExecutorCompany.Contains(request.ExecutorFilter));
        if (request.StartFrom.HasValue)
            query = query.Where(p => p.StartDate >= request.StartFrom.Value);
        if (request.StartTo.HasValue)
            query = query.Where(p => p.StartDate <= request.StartTo.Value);
        if (request.EndFrom.HasValue)
            query = query.Where(p => p.EndDate >= request.EndFrom.Value);
        if (request.EndTo.HasValue)
            query = query.Where(p => p.EndDate <= request.EndTo.Value);
        if (request.Priority.HasValue)
            query = query.Where(p => p.Priority == request.Priority.Value);

        query = request.SortBy?.ToLower() switch
        {
            "name" => request.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "startdate" => request.SortDescending ? query.OrderByDescending(p => p.StartDate) : query.OrderBy(p => p.StartDate),
            "enddate" => request.SortDescending ? query.OrderByDescending(p => p.EndDate) : query.OrderBy(p => p.EndDate),
            "priority" => request.SortDescending ? query.OrderByDescending(p => p.Priority) : query.OrderBy(p => p.Priority),
            _ => query.OrderBy(p => p.Name)
        };

        var projects = await query.ToListAsync(cancellationToken);
        return _mapper.Map<List<ProjectDto>>(projects);
    }
}