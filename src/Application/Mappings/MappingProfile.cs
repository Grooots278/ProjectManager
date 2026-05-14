using AutoMapper;
using ProjectManager.Application.Features.Employees.Queries.GetEmployees;
using ProjectManager.Application.Features.Projects.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.ProjectManagerFullName,
                opt => opt.MapFrom(src => src.ProjectManager.Name.GetFullName()))
            .ForMember(dest => dest.EmployeeEmails,
                opt => opt.MapFrom(src => src.ProjectEmployees
                    .Select(pe => pe.Employee.Email.ToString()).ToList()));

        CreateMap<Employee, EmployeeDto>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
            .ForMember(dest => dest.MiddleName, opt => opt.MapFrom(src => src.Name.MiddleName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));
    }
}