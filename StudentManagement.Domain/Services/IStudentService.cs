using StudentManagement.Domain.Entities;

namespace StudentManagement.Domain.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<int> CreateStudentAsync(Student student);
        Task<bool> UpdateStudentAsync(int id, Student student);
        Task<bool> DeleteStudentAsync(int id);
    }
}