namespace ITI.SMS.Application.Common.Interfaces;

public interface IRoleHierarchyScopeService
{
    Task<bool> HasAccessAsync(string userId, string[] roles, int? branchId, int? trackId, int? courseId);
}
