using FluentValidation;
using StudentManagement.API.Dtos;

namespace StudentManagement.API.Validators
{
    public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
    {
        public StudentCreateDtoValidator()
        {
            
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Student Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please provide a valid email address.");

            
            RuleFor(x => x.Age)
                .NotEmpty().WithMessage("Age is required.")
                .InclusiveBetween(18, 60).WithMessage("Age must be between 18 and 60 years.");

            
            RuleFor(x => x.Grade)
                .NotEmpty().WithMessage("Grade is required.")
                .Must(g => new[] { "A", "B", "C", "D", "F" }.Contains(g.ToUpper()))
                .WithMessage("Grade must be one of the following: A, B, C, D, F.");
        }
    }
}