using ITI.SMS.Application.Branches.DTOs;
using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using MediatR;
using System.Linq;

namespace ITI.SMS.Application.Users.Queries;

public class GetMyBranchQueryHandler : IRequestHandler<GetMyBranchQuery, BranchDto?>
{
    private readonly IBranchRepository _branchRepository;

    public GetMyBranchQueryHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<BranchDto?> Handle(GetMyBranchQuery request, CancellationToken cancellationToken)
    {
        var branches = await _branchRepository.GetAllAsync(cancellationToken);
        var branch = branches.FirstOrDefault(b => b.ManagerId == request.UserId);
        if (branch == null) return null;

        return new BranchDto
        {
            Id = branch.Id,
            Name = branch.Name,

            IsActive = branch.IsActive,
            ManagerId = branch.ManagerId,
            ManagerName = branch.Manager?.FullName
        };
    }
}
