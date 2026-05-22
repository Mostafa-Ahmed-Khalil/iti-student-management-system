using ITI.SMS.Domain.Entities;

namespace ITI.SMS.Domain.Interfaces;

public interface ITrackRepository
{
    Task<Track?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Track>> GetByBranchIdAsync(int branchId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Track>> GetBySupervisorIdAsync(string supervisorId, CancellationToken cancellationToken = default);
    Task<Track> AddAsync(Track track, CancellationToken cancellationToken = default);
    Task UpdateAsync(Track track, CancellationToken cancellationToken = default);
    Task DeleteAsync(Track track, CancellationToken cancellationToken = default);
}
