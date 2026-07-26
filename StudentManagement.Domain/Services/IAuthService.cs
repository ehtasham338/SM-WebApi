using StudentManagement.Domain.Entities;

namespace StudentManagement.Domain.Services
{
    public interface IAuthService
    {
        
        Task<int> RegisterAsync(User user, string plainPassword);

        
        Task<string> LoginAsync(string email, string password);
    }
}