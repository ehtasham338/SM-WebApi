using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Repositories;
using StudentManagement.Domain.Services;

namespace StudentManagement.Infrastructure.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        
        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
          
            return await _studentRepository.GetAllAsync();
        }



        public async Task<Student?> GetStudentByIdAsync(int id)
        {
          /*  if (id <= 0)
            {
                
                throw new ArgumentException("Invalid Student ID. ID must be greater than 0.");
            }

            
            if (id == 99)
            {
                throw new Exception("Database connection lost!");
            }*/

            return await _studentRepository.GetByIdAsync(id);
        }



        public async Task<int> CreateStudentAsync(Student student)
        {
            
            if (student.Age < 18)
            {
                throw new Exception("Student must be at least 18 years old.");
            }

            
            return await _studentRepository.AddAsync(student);
        }

        public async Task<bool> UpdateStudentAsync(int id, Student student)
        {
            
            if (id != student.Id)
            {
                throw new ArgumentException("ID mismatch");
            }
            return await _studentRepository.UpdateAsync(student);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var existingStudent = await _studentRepository.GetByIdAsync(id);
            if (existingStudent == null)
            {
                throw new KeyNotFoundException("Student not found to delete.");
            }

            return await _studentRepository.DeleteAsync(id);
        }
    }
}