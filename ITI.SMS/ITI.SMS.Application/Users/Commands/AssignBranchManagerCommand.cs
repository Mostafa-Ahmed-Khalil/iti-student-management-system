using MediatR;

namespace ITI.SMS.Application.Users.Commands;

public class AssignBranchManagerCommand : IRequest<Unit>
{
    public int BranchId { get; set; }
    public string UserId { get; set; } = string.Empty;
}
