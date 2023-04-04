using BlazorStudentApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorStudentApp.Data.Services
{
    public class StudentsService:IStudentsService
    {
        private readonly ApplicationDbContext _db;

        public StudentsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _db.Students.ToListAsync();
        }

        public async Task<Student> GetStudentAsync(int id)
        {
            return await _db.Students.FindAsync(id);
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            _db.Students.Add(student);
            await _db.SaveChangesAsync();
            return student;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            _db.Students.Update(student);
            await _db.SaveChangesAsync();
            return student;
        }

        public async Task<Student> DeleteStudentAsync(int id)
        {
            var student = await _db.Students.FindAsync(id);
            if (student != null)
            {
                _db.Students.Remove(student);
                await _db.SaveChangesAsync();
            }

            return student;
        }
    }
}
