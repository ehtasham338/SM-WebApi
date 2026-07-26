using StudentManagement.Domain.Entities;

namespace StudentManagement.Domain.Services
{
    public interface ITokenService
    {
        
        string CreateToken(User user);
    }
}