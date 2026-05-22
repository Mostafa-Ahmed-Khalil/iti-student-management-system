using ITI.SMS.Application.Common.Exceptions;
using ITI.SMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ITI.SMS.Application.Users.Commands;

public class AssignRolesCommandHandler : IRequestHandler<AssignRolesCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AssignRolesCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Unit> Handle(AssignRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.UserId} not found.");
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        
        // Remove roles not in the request
        var rolesToRemove = currentRoles.Except(request.Roles).ToList();
        if (rolesToRemove.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
        }

        // Add roles that are new
        var rolesToAdd = request.Roles.Except(currentRoles).ToList();
        foreach (var role in rolesToAdd)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
            else
            {
                throw new ValidationException($"Role {role} does not exist.");
            }
        }

        return Unit.Value;
    }
}
