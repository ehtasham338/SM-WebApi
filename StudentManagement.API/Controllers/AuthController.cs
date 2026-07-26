using AutoMapper; // Yeh add karein
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Dtos;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Services;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper; 

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
        {
            // AutoMappe
            var user = _mapper.Map<User>(request);

            
            int newId = await _authService.RegisterAsync(user, request.Password);

            return Ok(new
            {
                Message = "User registered successfully!",
                UserId = newId,
                CreatedAt = DateTime.Now
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto request)
        {
            
            string token = await _authService.LoginAsync(request.Email, request.Password);

            return Ok(new { Token = token });
        }
    }
}