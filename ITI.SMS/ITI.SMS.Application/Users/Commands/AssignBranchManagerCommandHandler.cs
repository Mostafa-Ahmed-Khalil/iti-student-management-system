using ITI.SMS.Application.Common.Exceptions;
using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ITI.SMS.Application.Users.Commands;

public class AssignBranchManagerCommandHandler : IRequestHandler<AssignBranchManagerCommand, Unit>
{
    private readonly IBranchRepository _branchRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public AssignBranchManagerCommandHandler(
        IBranchRepository branchRepository,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork)
    {
        _branchRepository = branchRepository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AssignBranchManagerCommand request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
        if (branch == null)
        {
            throw new NotFoundException($"Branch with ID {request.BranchId} not found.");
        }

        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.UserId} not found.");
        }

        var isManager = await _userManager.IsInRoleAsync(user, "Branch Manager");
        if (!isManager)
        {
            throw new ValidationException("User does not have the Branch Manager role.");
        }

        branch.ManagerId = user.Id;
        
        await _branchRepository.UpdateAsync(branch, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
