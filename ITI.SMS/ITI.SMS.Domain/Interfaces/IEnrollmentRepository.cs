using ITI.SMS.Domain.Entities;

namespace ITI.SMS.Domain.Interfaces;

public interface IEnrollmentRepository
{
    Task<IEnumerable<Enrollment>> GetByTrackIdAsync(int trackId, CancellationToken cancellationToken = default);
    Task<Enrollment?> GetAsync(int trackId, string studentId, CancellationToken cancellationToken = default);
    Task<Enrollment> AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Enrollment enrollment, CancellationToken cancellationToken = default);
}
