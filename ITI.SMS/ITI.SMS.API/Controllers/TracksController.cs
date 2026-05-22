using ITI.SMS.API.Attributes;
using ITI.SMS.Application.Tracks.Commands;
using Microsoft.AspNetCore.Mvc;

namespace ITI.SMS.API.Controllers;

[AuthorizeRole("Branch Manager", "Admin", "Technical Supervisor")]
public class TracksController : ApiControllerBase
{
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTrack(int id, [FromBody] UpdateTrackRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UpdateTrackCommand(id, request.Name, request.StartDate, request.IsActive, request.SupervisorId), cancellationToken);
        return Success<object?>(null, "Track updated successfully.");
    }

    [HttpDelete("{id}")]
    [AuthorizeRole("Branch Manager", "Admin")]
    public async Task<IActionResult> DeleteTrack(int id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteTrackCommand(id), cancellationToken);
        return Success<object?>(null, "Track deactivated successfully.");
    }

    [HttpGet("me")]
    [AuthorizeRole("Technical Supervisor")]
    public async Task<IActionResult> GetMyTracks(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await Mediator.Send(new ITI.SMS.Application.Tracks.Queries.GetMyTracksQuery(userId), cancellationToken);
        return Success(result, "Tracks retrieved successfully.");
    }

    [HttpGet("{trackId}/courses")]
    [AuthorizeRole("Technical Supervisor", "Branch Manager", "Admin")]
    public async Task<IActionResult> GetCourses(int trackId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ITI.SMS.Application.Courses.Queries.GetCoursesByTrackQuery(trackId), cancellationToken);
        return Success(result, "Courses retrieved successfully.");
    }

    [HttpPost("{trackId}/courses")]
    [AuthorizeRole("Technical Supervisor")]
    public async Task<IActionResult> CreateCourse(int trackId, [FromBody] ITI.SMS.Application.Courses.Commands.CreateCourseCommand command, CancellationToken cancellationToken)
    {
        command.TrackId = trackId;
        var result = await Mediator.Send(command, cancellationToken);
        
        return CreatedAtAction(nameof(GetCourses), new { trackId = trackId }, new ApiResponse<ITI.SMS.Application.Courses.DTOs.CourseDto>
        {
            Success = true,
            Data = result,
            Message = "Course created successfully."
        });
    }
}

public class UpdateTrackRequest
{
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public bool IsActive { get; set; }
    public string SupervisorId { get; set; } = string.Empty;
}
