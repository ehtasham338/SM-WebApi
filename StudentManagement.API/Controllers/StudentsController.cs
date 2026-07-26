using AutoMapper; // IMapper ke liye
using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Dtos;
using StudentManagement.Domain.Entities;
using StudentManagement.Domain.Services;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;

        public StudentsController(IStudentService studentService, IMapper mapper)
        {
            _studentService = studentService;
            _mapper = mapper;
        }

        // GET: api/students
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentService.GetAllStudentsAsync();

            // AutoMapper: List<Entity> to Dto
            var studentDtos = _mapper.Map<IEnumerable<StudentResponseDto>>(students);

            return Ok(studentDtos);
        }

        // GET: api/students/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();

            
            var studentDto = _mapper.Map<StudentResponseDto>(student);

            return Ok(studentDto);
        }


        // POST: api/students
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDto studentDto)
        {
            

            var studentEntity = _mapper.Map<Student>(studentDto);
            int newId = await _studentService.CreateStudentAsync(studentEntity);

            var responseDto = _mapper.Map<StudentResponseDto>(studentEntity);
            responseDto.Id = newId;

            return CreatedAtAction(nameof(GetStudentById), new { id = newId }, responseDto);
        }


        // PUT: api/students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentCreateDto studentDto)
        {
            // DTO ko Entity mein badla
            var studentEntity = _mapper.Map<Student>(studentDto);
            studentEntity.Id = id; 

            try
            {
                bool isUpdated = await _studentService.UpdateStudentAsync(id, studentEntity);
                if (!isUpdated) return StatusCode(500, "Error updating student.");
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                bool isDeleted = await _studentService.DeleteStudentAsync(id);
                if (!isDeleted) return StatusCode(500, "Error deleting student.");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}