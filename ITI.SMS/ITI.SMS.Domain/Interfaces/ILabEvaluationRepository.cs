using ITI.SMS.Domain.Entities;

namespace ITI.SMS.Domain.Interfaces;

public interface ILabEvaluationRepository
{
    Task<IEnumerable<LabEvaluation>> GetByCourseAsync(int courseId, CancellationToken cancellationToken = default);
    Task<LabEvaluation?> GetAsync(int courseId, string studentId, int labNumber, CancellationToken cancellationToken = default);
    Task<LabEvaluation> AddAsync(LabEvaluation evaluation, CancellationToken cancellationToken = default);
    Task UpdateAsync(LabEvaluation evaluation, CancellationToken cancellationToken = default);
}
