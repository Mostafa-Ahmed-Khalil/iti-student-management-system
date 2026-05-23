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
            .Include(c => c.Lecturer)
            .Include(c => c.CourseLabAssistants).ThenInclude(la => la.Instructor)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetByTrackIdAsync(int trackId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Include(c => c.Lecturer)
            .Include(c => c.CourseLabAssistants).ThenInclude(la => la.Instructor)
            .Where(c => c.TrackId == trackId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetByInstructorIdAsync(string instructorId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .Include(c => c.Track)
            .Include(c => c.CourseLabAssistants)
            .Where(c => (c.LecturerId == instructorId || c.CourseLabAssistants.Any(la => la.InstructorId == instructorId)) && c.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsAssignedToAsync(int courseId, string instructorId, CancellationToken cancellationToken = default)
    {
        return await _context.Courses
            .AnyAsync(c => c.Id == courseId && 
                           (c.LecturerId == instructorId || c.CourseLabAssistants.Any(la => la.InstructorId == instructorId)), 
                      cancellationToken);
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
