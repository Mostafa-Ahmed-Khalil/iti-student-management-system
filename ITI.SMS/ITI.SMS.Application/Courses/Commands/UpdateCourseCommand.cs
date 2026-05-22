using MediatR;
using ITI.SMS.Domain.Interfaces;

namespace ITI.SMS.Application.Courses.Commands;

public class UpdateCourseCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? InstructorId { get; set; }
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
        if (string.IsNullOrEmpty(request.InstructorId))
            throw new Common.Exceptions.ValidationException("An instructor must be assigned to the course.");

        var course = await _courseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (course == null)
            throw new Common.Exceptions.NotFoundException($"Course with ID {request.Id} not found.");

        course.Name = request.Name;
        course.InstructorId = string.IsNullOrEmpty(request.InstructorId) ? null : request.InstructorId;
        course.LectureHours = request.LectureHours;
        course.LabHours = request.LabHours;
        course.IsActive = request.IsActive;
        course.UpdatedAt = DateTime.UtcNow;

        await _courseRepository.UpdateAsync(course, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
