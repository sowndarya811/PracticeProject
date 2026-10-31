using Microsoft.AspNetCore.Mvc;
using PracticeProject.DTOs;
using PracticeProject.Services;

[ApiController]
[Route("api/students")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<StudentDto>>> GetAllStudents()
    {
        var students = await _studentService.GetAllStudentsAsync();

        return Ok(students);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<StudentDto>>> SearchStudents(
        [FromQuery] string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            return BadRequest("Search term is required.");

        var students =
            await _studentService.SearchStudentsByNameAsync(key);

        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudentById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Student ID must be greater than zero."
            });
        }

        var student = await _studentService.GetStudentByIdAsync(id);

        if (student is null)
        {
            return NotFound(new
            {
                message = "Student not found."
            });
        }

        return Ok(student);
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> AddStudent(
        CreateStudentDto studentDto)
    {
        if (await _studentService.EmailExistsAsync(studentDto.Email))
        {
            return Conflict(new
            {
                message = "A student with this email already exists."
            });
        }

        var createdStudent =
            await _studentService.AddStudentAsync(studentDto);

        return CreatedAtAction(
            nameof(GetStudentById),
            new { id = createdStudent.Id },
            createdStudent);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StudentDto>> UpdateStudent(
        int id,
        UpdateStudentDto studentDto)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Student ID must be greater than zero."
            });
        }

        if (await _studentService.EmailExistsAsync(
            studentDto.Email,
            id))
        {
            return Conflict(new
            {
                message = "Another student with this email already exists."
            });
        }

        var updatedStudent =
            await _studentService.UpdateStudentAsync(
                id,
                studentDto);

        if (updatedStudent is null)
        {
            return NotFound(new
            {
                message = "Student not found."
            });
        }

        return Ok(updatedStudent);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Student ID must be greater than zero."
            });
        }

        var deleted =
            await _studentService.DeleteStudentAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Student not found."
            });
        }

        return NoContent();
    }
}