using System.Collections.Generic;
using System.Threading.Tasks;
using SaveOnCloud.Core.Domain;
using SaveOnCloud.Core.Interfaces.Repositories;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace SaveOnCloud.Infrastructure.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SaveOnCloudDbContext _SaveOnCloudDbContext;

        public StudentRepository(SaveOnCloudDbContext dbContext)
        {
            _SaveOnCloudDbContext = dbContext;
        }

        public async Task<int> Add(Student student)
        {
            await _SaveOnCloudDbContext.Students.AddAsync(student);
            await _SaveOnCloudDbContext.SaveChangesAsync();
            return student.StudentId;
        }

        public async Task<Option<Student>> Get(int Id) =>
            await _SaveOnCloudDbContext.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .SingleOrDefaultAsync(s => s.StudentId == Id);

        public Task<List<Student>> GetAll() => 
            _SaveOnCloudDbContext.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .ToListAsync();

        public async Task Update(Student student)
        {
            _SaveOnCloudDbContext.Students.Update(student);
            await _SaveOnCloudDbContext.SaveChangesAsync();
        }

        public async Task Delete(int studentId)
        {
            var student = await _SaveOnCloudDbContext.Students.FindAsync(studentId);
            _SaveOnCloudDbContext.Students.Remove(student);
            await _SaveOnCloudDbContext.SaveChangesAsync();
        }
    }
}
