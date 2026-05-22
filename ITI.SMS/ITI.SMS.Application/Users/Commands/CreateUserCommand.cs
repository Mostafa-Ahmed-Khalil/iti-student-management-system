using ITI.SMS.Application.Users.DTOs;
using MediatR;

namespace ITI.SMS.Application.Users.Commands;

public class CreateUserCommand : IRequest<UserDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
