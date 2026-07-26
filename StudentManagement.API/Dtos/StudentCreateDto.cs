using System.ComponentModel.DataAnnotations;

namespace StudentManagement.API.Dtos
{
    public class StudentCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Grade { get; set; } = string.Empty;
       
    }
}