using ITI.SMS.API.Attributes;
using ITI.SMS.Application.Enrollments.Commands;
using ITI.SMS.Application.Enrollments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ITI.SMS.API.Controllers;

[AuthorizeRole("Technical Supervisor", "Admin")]
public class EnrollmentsController : ApiControllerBase
{
    [HttpGet("tracks/{trackId}/students")]
    public async Task<IActionResult> GetEnrolledStudents(int trackId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetEnrolledStudentsQuery(trackId), cancellationToken);
        return Success(result, "Students retrieved successfully.");
    }

    [HttpPost("tracks/{trackId}/enrollments")]
    public async Task<IActionResult> EnrollStudents(int trackId, [FromBody] EnrollStudentsRequest request, CancellationToken cancellationToken)
    {
        await Mediator.Send(new EnrollStudentsCommand(trackId, request.StudentIds), cancellationToken);
        return Success<object?>(null, "Students enrolled successfully.");
    }

    [HttpDelete("tracks/{trackId}/enrollments/{studentId}")]
    public async Task<IActionResult> UnenrollStudent(int trackId, string studentId, CancellationToken cancellationToken)
    {
        await Mediator.Send(new UnenrollStudentCommand(trackId, studentId), cancellationToken);
        return Success<object?>(null, "Student unenrolled successfully.");
    }
}

public class EnrollStudentsRequest
{
    public List<string> StudentIds { get; set; } = new();
}
