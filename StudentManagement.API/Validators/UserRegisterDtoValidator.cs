using FluentValidation;
using StudentManagement.API.Dtos;
using StudentManagement.Domain.Repositories;

namespace StudentManagement.API.Validators
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        
        private readonly IUserRepository _userRepository;

        public UserRegisterDtoValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            //  Full Name must bi at 5 char 
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required.")
                .MinimumLength(5).WithMessage("Full Name must be at least 5 characters.");

            // user anme must bi 5 char atleast 
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(5).WithMessage("Username must be at least 5 characters.")
                .MustAsync(async (username, cancellation) =>
                {
                    var existingUser = await _userRepository.GetUserByUsernameAsync(username);
                    return existingUser == null; 
                }).WithMessage("Username already exists. Please choose a different one.");

            // Email: Correct Format and Unique
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.")
                .MustAsync(async (email, cancellation) =>
                {
                    var existingUser = await _userRepository.GetUserByEmailAsync(email);
                    return existingUser == null;
                }).WithMessage("Email is already registered.");

            //  Phone Number: Correct Format and Unique
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Invalid phone number format.")
                .MustAsync(async (phone, cancellation) =>
                {
                    var existingUser = await _userRepository.GetUserByPhoneNumberAsync(phone);
                    return existingUser == null;
                }).WithMessage("Phone number is already registered.");

           
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            // Confirm Password: Match
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
        }
    }
}