using PracticeProject.DTOs;
using PracticeProject.Models;

namespace PracticeProject.Services
{
    public class StudentService : IStudentService
    {
        private readonly ILogger<StudentService> _logger;

        public StudentService(ILogger<StudentService> logger)
        {
            _logger = logger;
        }

        private readonly List<Student> _students = new()
        {
            new Student
            {
                Id = 1,
                Name = "Sowndarya",
                Email = "sowndarya@gmail.com",
                Age = 21
            },
            new Student
            {
                Id = 2,
                Name = "Priya",
                Email = "priya@gmail.com",
                Age = 22
            }
        };

        private int _nextId = 3;

        // GET: Get all students
        public List<StudentDto> GetAllStudents()
        {
            return _students.Select(s => new StudentDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Age = s.Age
            }).ToList();
        }

        // GET: Get student by ID
        public StudentDto? GetStudentById(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return null;

            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Age = student.Age
            };
        }

        // POST: Add student
        public StudentDto AddStudent(CreateStudentDto studentDto)
        {
            _logger.LogInformation(
                "Adding student with email {Email}",
                studentDto.Email);

            var student = new Student
            {
                Id = _nextId++,
                Name = studentDto.Name,
                Email = studentDto.Email,
                Age = studentDto.Age
            };

            _students.Add(student);

            _logger.LogInformation(
                "Student created successfully with ID {StudentId}",
                student.Id);

            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Age = student.Age
            };
        }

        // PUT: Update student
        public StudentDto? UpdateStudent(int id, UpdateStudentDto studentDto)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return null;

            student.Name = studentDto.Name;
            student.Email = studentDto.Email;
            student.Age = studentDto.Age;

            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email,
                Age = student.Age
            };
        }

        // DELETE: Delete student
        public bool DeleteStudent(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);

            if (student == null)
                return false;

            _students.Remove(student);

            return true;
        }

        // Check duplicate email
        public bool EmailExists(string email, int? excludeStudentId = null)
        {
            return _students.Any(s =>
                s.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                && s.Id != excludeStudentId);
        }
    }
}