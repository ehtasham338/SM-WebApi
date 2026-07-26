using AutoMapper;
using StudentManagement.API.Dtos;
using StudentManagement.Domain.Entities;

namespace StudentManagement.API.Mappings
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            //  Student (Entity) to StudentResponseDto  convert 
            CreateMap<Student, StudentResponseDto>();

            // StudentCreateDto to Student (Entity)  convert 
            CreateMap<StudentCreateDto, Student>();


            // regisatrion mapping
            CreateMap<UserRegisterDto, User>();
        }
    }
}