using PracticeProject.Models;

namespace PracticeProject.Repositories;

public interface IStudentRepository
{
    Task<List<Student>> GetAllAsync();

    Task<Student?> GetByIdAsync(int id);

    Task<bool> EmailExistsAsync(
        string email,
        int? excludeStudentId = null);

    Task AddAsync(Student student);

    Task UpdateAsync(Student student);

    Task DeleteAsync(Student student);
}