using Microsoft.Data.SqlClient;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Repositories;
using System.Data;

namespace StudentManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddUserAsync(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        
                        string userQuery = @"INSERT INTO Users (FullName, Username, Email, PhoneNumber, PasswordHash, CreatedAt, IsActive) 
                                             VALUES (@FullName, @Username, @Email, @PhoneNumber, @PasswordHash, @CreatedAt, @IsActive); 
                                             SELECT CAST(SCOPE_IDENTITY() as int);";

                        using (var userCmd = new SqlCommand(userQuery, connection, transaction))
                        {
                            userCmd.Parameters.Add(new SqlParameter("@FullName", SqlDbType.NVarChar) { Value = user.FullName });
                            userCmd.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar) { Value = user.Username });
                            userCmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = user.Email });
                            userCmd.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.NVarChar) { Value = user.PhoneNumber });
                            userCmd.Parameters.Add(new SqlParameter("@PasswordHash", SqlDbType.NVarChar) { Value = user.PasswordHash });
                            userCmd.Parameters.Add(new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = DateTime.Now });
                            userCmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = true });

                            int userId = (int)await userCmd.ExecuteScalarAsync();

                            // 2. Default Role
                            await AssignRoleAsyncInternal(userId, 2, connection, transaction);

                            transaction.Commit();
                            return userId;
                        }
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // Helper method for transaction
        private async Task AssignRoleAsyncInternal(int userId, int roleId, SqlConnection connection, SqlTransaction transaction)
        {
            string roleQuery = "INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)";
            using (var roleCmd = new SqlCommand(roleQuery, connection, transaction))
            {
                roleCmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int) { Value = userId });
                roleCmd.Parameters.Add(new SqlParameter("@RoleId", SqlDbType.Int) { Value = roleId });
                await roleCmd.ExecuteNonQueryAsync();
            }
        }

        public async Task AssignRoleAsync(int userId, int roleId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await AssignRoleAsyncInternal(userId, roleId, connection, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // Registration time username check 
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            User? user = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, Username FROM Users WHERE Username = @Username";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Username", SqlDbType.NVarChar) { Value = username });
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User { Id = reader.GetInt32("Id"), Username = reader.GetString("Username") };
                        }
                    }
                }
            }
            return user;
        }

        // Login time emial to role 
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            User? user = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                // JOIN l
                string query = @"
                    SELECT u.Id, u.FullName, u.Username, u.Email, u.PhoneNumber, u.PasswordHash, r.Name AS Role, u.IsActive 
                    FROM Users u
                    INNER JOIN UserRoles ur ON u.Id = ur.UserId
                    INNER JOIN Roles r ON ur.RoleId = r.Id
                    WHERE u.Email = @Email AND u.IsActive = 1";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar) { Value = email });
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32("Id"),
                                FullName = reader.GetString("FullName"),
                                Username = reader.GetString("Username"),
                                Email = reader.GetString("Email"),
                                PhoneNumber = reader.GetString("PhoneNumber"),
                                PasswordHash = reader.GetString("PasswordHash"),
                                Role = reader.GetString("Role"),
                                IsActive = reader.GetBoolean("IsActive")
                            };
                        }
                    }
                }
            }
            return user;
        }


        
        public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            User? user = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id, PhoneNumber FROM Users WHERE PhoneNumber = @PhoneNumber";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@PhoneNumber", SqlDbType.NVarChar) { Value = phoneNumber });
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User { Id = reader.GetInt32("Id") };
                        }
                    }
                }
            }
            return user;
        }
    }
}