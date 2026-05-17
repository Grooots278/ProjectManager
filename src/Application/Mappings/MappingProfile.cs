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
            .ConstructUsing(src => new ProjectDto(
                src.Id,
                src.Name,
                src.CustomerCompany,
                src.ExecutorCompany,
                src.ProjectManager.Id,  
                null!,                   
                src.StartDate,
                src.EndDate,
                src.Priority,
                null!                    
            ))
            .ForMember(dest => dest.ProjectManagerFullName,
                opt => opt.MapFrom(src => src.ProjectManager.Name.GetFullName()))
            .ForMember(dest => dest.EmployeeEmails,
                opt => opt.MapFrom(src => src.ProjectEmployees
            .Select(pe => pe.Employee.Email.ToString()).ToList()));

        CreateMap<Employee, EmployeeDto>()
            .ConstructUsing(src => new EmployeeDto(
                src.Id,
                src.Name.FirstName,
                src.Name.LastName,
                src.Name.MiddleName,
                src.Email.Value
            ))
            .ForMember(dest => dest.FirstName, opt => opt.Ignore())
            .ForMember(dest => dest.LastName, opt => opt.Ignore())
            .ForMember(dest => dest.MiddleName, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.Ignore());
    }
}