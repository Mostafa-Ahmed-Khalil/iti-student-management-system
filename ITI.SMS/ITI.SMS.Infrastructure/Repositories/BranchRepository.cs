using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using ITI.SMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.SMS.Infrastructure.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly AppDbContext _context;

    public BranchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Branch?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Branches
            .Include(b => b.Manager)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Branch>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Branches
            .Include(b => b.Manager)
            .ToListAsync(cancellationToken);
    }

    public async Task<Branch> AddAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        await _context.Branches.AddAsync(branch, cancellationToken);
        return branch;
    }

    public Task UpdateAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        _context.Branches.Update(branch);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        _context.Branches.Remove(branch);
        return Task.CompletedTask;
    }
}
