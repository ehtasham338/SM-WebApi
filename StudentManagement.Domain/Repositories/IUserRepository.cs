using StudentManagement.Domain.Entities;

namespace StudentManagement.Domain.Repositories
{
    public interface IUserRepository
    {
        
        Task<int> AddUserAsync(User user);

        
        Task AssignRoleAsync(int userId, int roleId);

        
        Task<User?> GetUserByUsernameAsync(string username);

       
        Task<User?> GetUserByEmailAsync(string email);
        
        Task<User?> GetUserByPhoneNumberAsync(string phoneNumber);
    }
}