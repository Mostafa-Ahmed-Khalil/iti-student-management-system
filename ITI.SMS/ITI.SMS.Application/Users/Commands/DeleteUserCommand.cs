using ITI.SMS.Application.Common.Exceptions;
using ITI.SMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ITI.SMS.Application.Users.Commands;

public class DeleteUserCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId)
            ?? throw new NotFoundException($"User {request.UserId} not found.");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return Unit.Value;
    }
}
