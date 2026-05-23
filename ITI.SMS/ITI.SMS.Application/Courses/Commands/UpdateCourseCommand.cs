using MediatR;
using ITI.SMS.Domain.Interfaces;
using ITI.SMS.Domain.Entities;

namespace ITI.SMS.Application.Courses.Commands;

public class UpdateCourseCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? LecturerId { get; set; }
    public List<string> LabAssistantIds { get; set; } = new();
    public int LectureHours { get; set; }
    public int LabHours { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.LecturerId))
            throw new Common.Exceptions.ValidationException("A lecturer must be assigned to the course.");

        var course = await _courseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (course == null)
            throw new Common.Exceptions.NotFoundException($"Course with ID {request.Id} not found.");

        course.Name = request.Name;
        course.LecturerId = string.IsNullOrEmpty(request.LecturerId) ? null : request.LecturerId;
        course.LectureHours = request.LectureHours;
        course.LabHours = request.LabHours;
        course.IsActive = request.IsActive;
        course.UpdatedAt = DateTime.UtcNow;

        // Update Lab Assistants
        var existingLabAssistants = course.CourseLabAssistants.Select(x => x.InstructorId).ToList();
        var toAdd = request.LabAssistantIds.Except(existingLabAssistants).ToList();
        var toRemove = existingLabAssistants.Except(request.LabAssistantIds).ToList();

        foreach (var id in toRemove)
        {
            var item = course.CourseLabAssistants.First(x => x.InstructorId == id);
            course.CourseLabAssistants.Remove(item);
        }

        foreach (var id in toAdd)
        {
            course.CourseLabAssistants.Add(new CourseLabAssistant { InstructorId = id });
        }

        await _courseRepository.UpdateAsync(course, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
