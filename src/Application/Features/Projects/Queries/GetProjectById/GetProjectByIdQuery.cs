using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.Features.Projects.DTOs;

namespace ProjectManager.Application.Features.Projects.Queries.GetProjectById;

public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectDto?>;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProjectByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProjectDto?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await _context.Projects
            .Include(p => p.ProjectManager)
            .Include(p => p.ProjectEmployees)
                .ThenInclude(pe => pe.Employee)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        return project is null ? null : _mapper.Map<ProjectDto>(project);
    }
}