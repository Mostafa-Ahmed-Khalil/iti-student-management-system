using ITI.SMS.Domain.Entities;

namespace ITI.SMS.Domain.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> GetByTrackIdAsync(int trackId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> GetByInstructorIdAsync(string instructorId, CancellationToken cancellationToken = default);
    Task<bool> IsAssignedToAsync(int courseId, string instructorId, CancellationToken cancellationToken = default);
    Task<Course> AddAsync(Course course, CancellationToken cancellationToken = default);
    Task UpdateAsync(Course course, CancellationToken cancellationToken = default);
    Task DeleteAsync(Course course, CancellationToken cancellationToken = default);
}
