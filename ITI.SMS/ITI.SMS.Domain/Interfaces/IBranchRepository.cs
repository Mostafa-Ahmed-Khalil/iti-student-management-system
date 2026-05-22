using ITI.SMS.Domain.Entities;

namespace ITI.SMS.Domain.Interfaces;

public interface IBranchRepository
{
    Task<Branch?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Branch>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Branch> AddAsync(Branch branch, CancellationToken cancellationToken = default);
    Task UpdateAsync(Branch branch, CancellationToken cancellationToken = default);
    Task DeleteAsync(Branch branch, CancellationToken cancellationToken = default);
}
