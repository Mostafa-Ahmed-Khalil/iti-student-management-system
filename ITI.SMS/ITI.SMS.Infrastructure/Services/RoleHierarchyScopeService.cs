using ITI.SMS.Application.Common.Interfaces;

namespace ITI.SMS.Infrastructure.Services;

public class RoleHierarchyScopeService : IRoleHierarchyScopeService
{
    public Task<bool> HasAccessAsync(string userId, string[] roles, int? branchId, int? trackId, int? courseId)
    {
        // Admin role bypasses all hierarchical scope checks
        if (roles.Contains("Admin", StringComparer.OrdinalIgnoreCase))
        {
            return Task.FromResult(true);
        }

        // TODO: (Epics 2 & 3) Implement actual database lookups for Branch Manager, Supervisor, and Instructor scopes.
        // For now, allow access so the MVP skeleton endpoints work, or return false if we want strictness early.
        // Returning true here acts as a stub until the domain entities and assignments are created.
        
        return Task.FromResult(true);
    }
}
