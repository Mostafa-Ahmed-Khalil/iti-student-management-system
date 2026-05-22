using ITI.SMS.API.Attributes;
using ITI.SMS.Application.Branches.Commands;
using ITI.SMS.Application.Branches.DTOs;
using ITI.SMS.Application.Branches.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ITI.SMS.API.Controllers;

[AuthorizeRole("Admin")]
public class BranchesController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBranches(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetBranchesQuery(), cancellationToken);
        return Success(result, "Branches retrieved successfully.");
    }

    [HttpPost]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBranchCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(GetBranches), new { id = result.Id }, new ApiResponse<BranchDto>
        {
            Success = true,
            Data = result,
            Message = "Branch created successfully."
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBranch(int id, [FromBody] UpdateBranchRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UpdateBranchCommand(id, request.Name, request.Location), cancellationToken);
        return Success<object?>(null, "Branch updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBranch(int id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteBranchCommand(id), cancellationToken);
        return Success<object?>(null, "Branch deleted successfully.");
    }
}
