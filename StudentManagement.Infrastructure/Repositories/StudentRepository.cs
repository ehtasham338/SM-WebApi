using Microsoft.Data.SqlClient;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Repositories;
using System.Data;

namespace StudentManagement.Infrastructure.Repositories
{
    
    public class StudentRepository : IStudentRepository
    {
        // conncetion string store 
        private readonly string _connectionString;

        
        public StudentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Get All Student
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            var students = new List<Student>();

            
            using (var connection = new SqlConnection(_connectionString))
            {
                // SQL Query
                string query = "SELECT Id, Name, Email, Age, Grade FROM Students";

                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();

                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        
                        while (await reader.ReadAsync())
                        {
                            students.Add(new Student
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Email = reader.GetString("Email"),
                                Age = reader.GetInt32("Age"),
                                Grade = reader.GetString("Grade")
                            });
                        }
                    }
                }
            }

            return students;
        }

        // get single studnet 
        public async Task<Student?> GetByIdAsync(int id)
        {
            Student? student = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Name, Email, Age, Grade FROM Students WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync()) 
                        {
                            student = new Student
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Email = reader.GetString("Email"),
                                Age = reader.GetInt32("Age"),
                                Grade = reader.GetString("Grade")
                            };
                        }
                    }
                }
            }

            return student;
        }


        // student add karne ka method
        public async Task<int> AddAsync(Student student)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // SQL Query: SCOPE_IDENTITY() naye banaye gaye record ka Id return karta hai
                string query = "INSERT INTO Students (Name, Email, Age, Grade) VALUES (@Name, @Email, @Age, @Grade); SELECT CAST(SCOPE_IDENTITY() as int);";

                using (var command = new SqlCommand(query, connection))
                {
                    
                    command.Parameters.Add(new SqlParameter("@Name", System.Data.SqlDbType.NVarChar) { Value = student.Name });
                    command.Parameters.Add(new SqlParameter("@Email", System.Data.SqlDbType.NVarChar) { Value = student.Email });
                    command.Parameters.Add(new SqlParameter("@Age", System.Data.SqlDbType.Int) { Value = student.Age });
                    command.Parameters.Add(new SqlParameter("@Grade", System.Data.SqlDbType.NVarChar) { Value = student.Grade });

                    await connection.OpenAsync();

                    
                    int newId = (int)await command.ExecuteScalarAsync();
                    return newId;
                }
            }
        }

        // Student update
        public async Task<bool> UpdateAsync(Student student)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Students SET Name = @Name, Email = @Email, Age = @Age, Grade = @Grade WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int) { Value = student.Id });
                    command.Parameters.Add(new SqlParameter("@Name", System.Data.SqlDbType.NVarChar) { Value = student.Name });
                    command.Parameters.Add(new SqlParameter("@Email", System.Data.SqlDbType.NVarChar) { Value = student.Email });
                    command.Parameters.Add(new SqlParameter("@Age", System.Data.SqlDbType.Int) { Value = student.Age });
                    command.Parameters.Add(new SqlParameter("@Grade", System.Data.SqlDbType.NVarChar) { Value = student.Grade });

                    await connection.OpenAsync();

                    // ExecuteNonQueryAsync batata hai ke kitni rows affect hui hain
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0; 
                }
            }
        }

        // Student delete 
        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Students WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.Int) { Value = id });

                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }
}