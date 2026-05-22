using ITI.SMS.API.Attributes;
using ITI.SMS.Application.Branches.Commands;
using ITI.SMS.Application.Branches.DTOs;
using ITI.SMS.Application.Branches.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ITI.SMS.API.Controllers;

using Microsoft.AspNetCore.Authorization;

[Authorize]
public class BranchesController : ApiControllerBase
{
    [AuthorizeRole("Admin", "Branch Manager")]
    [HttpGet]
    public async Task<IActionResult> GetBranches(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetBranchesQuery(), cancellationToken);
        return Success(result, "Branches retrieved successfully.");
    }

    [AuthorizeRole("Admin")]
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

    [AuthorizeRole("Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBranch(int id, [FromBody] UpdateBranchRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UpdateBranchCommand(id, request.Name, request.Location), cancellationToken);
        return Success<object?>(null, "Branch updated successfully.");
    }

    [AuthorizeRole("Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBranch(int id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteBranchCommand(id), cancellationToken);
        return Success<object?>(null, "Branch deactivated successfully.");
    }

    [AuthorizeRole("Admin")]
    [HttpPut("{id}/reactivate")]
    public async Task<IActionResult> ReactivateBranch(int id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new ITI.SMS.Application.Branches.Commands.ReactivateBranchCommand(id), cancellationToken);
        return Success<object?>(null, "Branch reactivated successfully.");
    }

    [AuthorizeRole("Admin")]
    [HttpPost("{id}/managers")]
    public async Task<IActionResult> AssignBranchManager(int id, [FromBody] AssignManagerRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new ITI.SMS.Application.Users.Commands.AssignBranchManagerCommand { BranchId = id, UserId = request.UserId }, cancellationToken);
        return Success<object?>(null, "Branch manager assigned successfully.");
    }

    [AuthorizeRole("Branch Manager")]
    [HttpGet("{branchId}/tracks")]
    public async Task<IActionResult> GetTracks(int branchId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ITI.SMS.Application.Tracks.Queries.GetTracksByBranchQuery(branchId), cancellationToken);
        return Success(result, "Tracks retrieved successfully.");
    }

    [AuthorizeRole("Branch Manager")]
    [HttpPost("{branchId}/tracks")]
    public async Task<IActionResult> CreateTrack(int branchId, [FromBody] CreateTrackRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ITI.SMS.Application.Tracks.Commands.CreateTrackCommand 
        { 
            BranchId = branchId, 
            Name = request.Name, 
            StartDate = request.StartDate 
        }, cancellationToken);
        
        return CreatedAtAction(nameof(GetTracks), new { branchId = branchId }, new ApiResponse<ITI.SMS.Application.Tracks.DTOs.TrackDto>
        {
            Success = true,
            Data = result,
            Message = "Track created successfully."
        });
    }
}

public class AssignManagerRequest
{
    public string UserId { get; set; } = string.Empty;
}

public class CreateTrackRequest
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
}
