using ITI.SMS.Application.Users.DTOs;
using MediatR;

namespace ITI.SMS.Application.Users.Queries;

public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
