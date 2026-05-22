using ITI.SMS.Application.Common.Exceptions;
using ITI.SMS.Application.Users.DTOs;
using ITI.SMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ITI.SMS.Application.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new ValidationException("User with this email already exists.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ValidationException($"User creation failed: {errors}");
        }

        var rolesAdded = new List<string>();
        foreach (var role in request.Roles)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);
                rolesAdded.Add(role);
            }
        }

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FullName = user.FullName ?? string.Empty,
            Roles = rolesAdded
        };
    }
}
