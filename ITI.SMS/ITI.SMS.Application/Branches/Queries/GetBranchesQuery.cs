using ITI.SMS.Application.Branches.DTOs;
using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Branches.Queries;

public record GetBranchesQuery : IRequest<IEnumerable<BranchDto>>;

public class GetBranchesQueryHandler : IRequestHandler<GetBranchesQuery, IEnumerable<BranchDto>>
{
    private readonly IBranchRepository _branchRepository;

    public GetBranchesQueryHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<IEnumerable<BranchDto>> Handle(GetBranchesQuery request, CancellationToken cancellationToken)
    {
        var branches = await _branchRepository.GetAllAsync(cancellationToken);
        
        return branches.Select(b => new BranchDto
        {
            Id = b.Id,
            Name = b.Name,

            IsActive = b.IsActive,
            ManagerId = b.ManagerId,
            ManagerName = b.Manager?.FullName
        });
    }
}
