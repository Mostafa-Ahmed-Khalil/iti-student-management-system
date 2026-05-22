using ITI.SMS.Application.Common.Exceptions;
using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Branches.Commands;

public record UpdateBranchCommand(int Id, string Name) : IRequest;

public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBranchCommandHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (branch == null)
        {
            throw new NotFoundException($"Branch with ID {request.Id} not found.");
        }

        branch.Name = request.Name;


        await _branchRepository.UpdateAsync(branch, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
