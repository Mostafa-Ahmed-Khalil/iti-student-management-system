using ITI.SMS.API.Attributes;
using ITI.SMS.Application.Users.Commands;
using ITI.SMS.Application.Users.DTOs;
using ITI.SMS.Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ITI.SMS.API.Controllers;

using Microsoft.AspNetCore.Authorization;

[Authorize]
public class UsersController : ApiControllerBase
{
    [AuthorizeRole("Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 50, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetUsersQuery { Page = page, PageSize = pageSize }, cancellationToken);
        return Success(result, "Users retrieved successfully.");
    }

    [AuthorizeRole("Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(GetUsers), new { id = result.Id }, new ApiResponse<UserDto>
        {
            Success = true,
            Data = result,
            Message = "User created successfully."
        });
    }

    [AuthorizeRole("Admin")]
    [HttpPut("{id}/roles")]
    public async Task<IActionResult> AssignRoles(string id, [FromBody] List<string> roles, CancellationToken cancellationToken)
    {
        await Mediator.Send(new AssignRolesCommand { UserId = id, Roles = roles }, cancellationToken);
        return Success<object?>(null, "Roles assigned successfully.");
    }

    [AuthorizeRole("Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        command.UserId = id;
        var result = await Mediator.Send(command, cancellationToken);
        return Success(result, "User updated successfully.");
    }

    [AuthorizeRole("Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteUserCommand { UserId = id }, cancellationToken);
        return Success<object?>(null, "User deleted successfully.");
    }

    [HttpGet("me/branch")]
    [AuthorizeRole("Branch Manager")]
    public async Task<IActionResult> GetMyBranch(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await Mediator.Send(new ITI.SMS.Application.Users.Queries.GetMyBranchQuery { UserId = userId }, cancellationToken);
        return Success(result, "Branch retrieved successfully.");
    }
}
