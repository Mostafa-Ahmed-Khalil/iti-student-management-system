using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using ITI.SMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.SMS.Infrastructure.Repositories;

public class LabEvaluationRepository : ILabEvaluationRepository
{
    private readonly AppDbContext _context;

    public LabEvaluationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LabEvaluation>> GetByCourseAsync(int courseId, CancellationToken cancellationToken = default)
    {
        return await _context.LabEvaluations
            .Include(le => le.Student)
            .Where(le => le.CourseId == courseId)
            .ToListAsync(cancellationToken);
    }

    public async Task<LabEvaluation?> GetAsync(int courseId, string studentId, int labNumber, CancellationToken cancellationToken = default)
    {
        return await _context.LabEvaluations
            .FirstOrDefaultAsync(le => le.CourseId == courseId && le.StudentId == studentId && le.LabNumber == labNumber, cancellationToken);
    }

    public async Task<LabEvaluation> AddAsync(LabEvaluation evaluation, CancellationToken cancellationToken = default)
    {
        await _context.LabEvaluations.AddAsync(evaluation, cancellationToken);
        return evaluation;
    }

    public Task UpdateAsync(LabEvaluation evaluation, CancellationToken cancellationToken = default)
    {
        evaluation.LastUpdatedAt = DateTime.UtcNow;
        _context.LabEvaluations.Update(evaluation);
        return Task.CompletedTask;
    }
}
