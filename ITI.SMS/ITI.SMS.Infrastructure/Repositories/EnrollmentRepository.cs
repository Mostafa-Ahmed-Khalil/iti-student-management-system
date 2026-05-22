using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using ITI.SMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.SMS.Infrastructure.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AppDbContext _context;

    public EnrollmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Enrollment>> GetByTrackIdAsync(int trackId, CancellationToken cancellationToken = default)
    {
        return await _context.Enrollments
            .Include(e => e.Student)
            .Where(e => e.TrackId == trackId && e.IsActive)
            .ToListAsync(cancellationToken);
    }

    public async Task<Enrollment?> GetAsync(int trackId, string studentId, CancellationToken cancellationToken = default)
    {
        return await _context.Enrollments
            .FirstOrDefaultAsync(e => e.TrackId == trackId && e.StudentId == studentId, cancellationToken);
    }

    public async Task<Enrollment> AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        await _context.Enrollments.AddAsync(enrollment, cancellationToken);
        return enrollment;
    }

    public Task DeleteAsync(Enrollment enrollment, CancellationToken cancellationToken = default)
    {
        enrollment.IsActive = false;
        _context.Enrollments.Update(enrollment);
        return Task.CompletedTask;
    }
}
