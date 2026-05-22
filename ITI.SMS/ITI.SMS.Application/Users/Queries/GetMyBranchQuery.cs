using ITI.SMS.Application.Branches.DTOs;
using MediatR;

namespace ITI.SMS.Application.Users.Queries;

public class GetMyBranchQuery : IRequest<BranchDto?>
{
    public string UserId { get; set; } = string.Empty;
}
