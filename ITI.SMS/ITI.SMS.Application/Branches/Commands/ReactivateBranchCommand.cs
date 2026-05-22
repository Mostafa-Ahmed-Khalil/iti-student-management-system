using ITI.SMS.Application.Common.Exceptions;
using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Branches.Commands;

public record ReactivateBranchCommand(int Id) : IRequest;

public class ReactivateBranchCommandHandler : IRequestHandler<ReactivateBranchCommand>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReactivateBranchCommandHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ReactivateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (branch == null)
        {
            throw new NotFoundException($"Branch with ID {request.Id} not found.");
        }

        branch.IsActive = true; 

        await _branchRepository.UpdateAsync(branch, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
