using MediatR;

namespace ITI.SMS.Application.Users.Commands;

public class AssignRolesCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}
