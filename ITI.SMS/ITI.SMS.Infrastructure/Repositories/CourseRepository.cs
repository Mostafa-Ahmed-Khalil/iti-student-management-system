using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using ITI.SMS.Infrastructure.Data;

namespace ITI.SMS.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Include(c => c.Instructor)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetByTrackIdAsync(int trackId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Include(c => c.Instructor)
            .Where(c => c.TrackId == trackId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetByInstructorIdAsync(string instructorId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Include(c => c.Track)
            .Where(c => c.InstructorId == instructorId && c.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Course> AddAsync(Course course, CancellationToken cancellationToken = default)
    {
        await _context.Courses.AddAsync(course, cancellationToken);
        return course;
    }

    public Task UpdateAsync(Course course, CancellationToken cancellationToken = default)
    {
        _context.Courses.Update(course);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Course course, CancellationToken cancellationToken = default)
    {
        _context.Courses.Remove(course);
        return Task.CompletedTask;
    }
}
