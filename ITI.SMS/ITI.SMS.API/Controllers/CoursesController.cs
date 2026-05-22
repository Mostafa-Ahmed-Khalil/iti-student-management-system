using ITI.SMS.API.Attributes;
using ITI.SMS.Application.Courses.Commands;
using Microsoft.AspNetCore.Mvc;

namespace ITI.SMS.API.Controllers;

[AuthorizeRole("Technical Supervisor")]
public class CoursesController : ApiControllerBase
{
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        await Mediator.Send(command, cancellationToken);
        return Success<object?>(null, "Course updated successfully.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCourse(int id, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteCourseCommand(id), cancellationToken);
        return Success<object?>(null, "Course deactivated successfully.");
    }
}
