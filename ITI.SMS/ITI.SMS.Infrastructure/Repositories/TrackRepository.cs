using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

using ITI.SMS.Infrastructure.Data;

namespace ITI.SMS.Infrastructure.Repositories;

public class TrackRepository : ITrackRepository
{
    private readonly AppDbContext _context;

    public TrackRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Track?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Tracks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Track>> GetByBranchIdAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return await _context.Tracks
            .Where(t => t.BranchId == branchId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Track> AddAsync(Track track, CancellationToken cancellationToken = default)
    {
        await _context.Tracks.AddAsync(track, cancellationToken);
        return track;
    }

    public Task UpdateAsync(Track track, CancellationToken cancellationToken = default)
    {
        _context.Tracks.Update(track);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Track track, CancellationToken cancellationToken = default)
    {
        _context.Tracks.Remove(track);
        return Task.CompletedTask;
    }
}
