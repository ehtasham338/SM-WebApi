using StudentManagement.Domain.Entities;

namespace StudentManagement.Domain.Repositories
{
    
    public interface IStudentRepository
    {
        // All Student 
        Task<IEnumerable<Student>> GetAllAsync();

        // Get single studnet 
        Task<Student?> GetByIdAsync(int id);

        // Add new studnet 
        Task<int> AddAsync(Student student);

        // Studen upadte detils 
        Task<bool> UpdateAsync(Student student);

        // Student delete 
        Task<bool> DeleteAsync(int id);
    }
}