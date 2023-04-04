using BlazorStudentApp.Data.Models;

namespace BlazorStudentApp.Data.Services
{
    public interface IStudentsService
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentAsync(int id);
        Task<Student> AddStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student);
        Task<Student> DeleteStudentAsync(int id);
    }
}
