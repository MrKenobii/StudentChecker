using AutoMapper;
using Backend.DataTransferObject;
using Backend.Models;

namespace Backend.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Course, CourseDto>();
        CreateMap<City, CityDto>();
        CreateMap<College, CollegeDto>();
        CreateMap<Student, StudentDto>();
        CreateMap<Company, CompanyDto>();
        CreateMap<Recruiter, RecruiterDto>();
    }
    
}