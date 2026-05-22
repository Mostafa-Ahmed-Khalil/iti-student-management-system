using ITI.SMS.API.Attributes;
using ITI.SMS.Application.Tracks.Commands;
using Microsoft.AspNetCore.Mvc;

namespace ITI.SMS.API.Controllers;

[AuthorizeRole("Branch Manager")]
public class TracksController : ApiControllerBase
{
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrack(int id, [FromBody] UpdateTrackRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UpdateTrackCommand(id, request.Name, request.StartDate, request.IsActive, request.SupervisorId), cancellationToken);
        return Success<object?>(null, "Track updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrack(int id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteTrackCommand(id), cancellationToken);
        return Success<object?>(null, "Track deactivated successfully.");
    }
}

public class UpdateTrackRequest
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public bool IsActive { get; set; }
    public string SupervisorId { get; set; } = string.Empty;
}
