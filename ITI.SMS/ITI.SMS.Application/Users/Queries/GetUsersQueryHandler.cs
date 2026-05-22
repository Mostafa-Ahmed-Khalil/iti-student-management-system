using ITI.SMS.Application.Users.DTOs;
using ITI.SMS.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;



namespace ITI.SMS.Application.Users.Queries;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUsersQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var usersWithRoles = _userManager.Users
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(u => new
            {
                User = u,
                Roles = u.UserRoles.Select(ur => ur.Role.Name!).ToList()
            })
            .ToList();

        var userDtos = usersWithRoles.Select(u => new UserDto
        {
            Id = u.User.Id,
            Email = u.User.Email!,
            FullName = u.User.FullName ?? string.Empty,
            Roles = u.Roles
        }).ToList();

        return userDtos;
    }
}
