using ITI.SMS.API.Attributes;
using ITI.SMS.Application.Courses.Commands;
using ITI.SMS.Application.Instructor.Commands;
using ITI.SMS.Application.Instructor.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ITI.SMS.API.Controllers;

[AuthorizeRole("Technical Supervisor", "Instructor")]
public class CoursesController : ApiControllerBase
{
    [AuthorizeRole("Instructor")]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyCourses(CancellationToken cancellationToken)
    {
        var instructorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(instructorId)) return Unauthorized();

        var result = await Mediator.Send(new GetMyCoursesQuery(instructorId), cancellationToken);
        return Success(result, "Courses retrieved successfully.");
    }

    [AuthorizeRole("Instructor")]
    [HttpGet("{courseId}/evaluations")]
    public async Task<IActionResult> GetLabEvaluations(int courseId, [FromQuery] int trackId, CancellationToken cancellationToken)
    {
        var isInstructor = User.IsInRole("Instructor");
        var result = await Mediator.Send(new GetLabEvaluationsQuery(courseId, trackId, isInstructor), cancellationToken);
        return Success(result, "Evaluations retrieved successfully.");
    }

    [AuthorizeRole("Instructor")]
    [HttpPut("{courseId}/evaluations")]
    public async Task<IActionResult> UpsertLabEvaluation(int courseId, [FromBody] UpsertLabEvaluationCommand command, CancellationToken cancellationToken)
    {
        command.CourseId = courseId;
        await Mediator.Send(command, cancellationToken);
        return Success<object?>(null, "Evaluation saved successfully.");
    }

    [AuthorizeRole("Technical Supervisor")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        await Mediator.Send(command, cancellationToken);
        return Success<object?>(null, "Course updated successfully.");
    }

    [AuthorizeRole("Technical Supervisor")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteCourseCommand(id), cancellationToken);
        return Success<object?>(null, "Course deactivated successfully.");
    }
}
