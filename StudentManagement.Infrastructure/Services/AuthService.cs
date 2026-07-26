using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Repositories;
using StudentManagement.Domain.Services;

namespace StudentManagement.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<int> RegisterAsync(User user, string plainPassword)
        {
            //  Check username exist 
            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists."); 
            }

            // 2. Password Hash (Encrypt) 
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            //  save in nDB 
            int newId = await _userRepository.AddUserAsync(user);

            return newId;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            // 1. User DB to  Email 
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            //  Extra safety check
            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Your account is deactivated. Please contact admin.");
            }

            //  Password verify 
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            //  Token generat
            string token = _tokenService.CreateToken(user);

            return token;
        }
    }
}