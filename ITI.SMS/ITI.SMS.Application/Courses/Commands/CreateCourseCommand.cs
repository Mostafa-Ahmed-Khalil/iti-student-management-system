using ITI.SMS.Application.Courses.DTOs;
using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Courses.Commands;

public class CreateCourseCommand : IRequest<CourseDto>
{
    public string Name { get; set; } = string.Empty;
    public int TrackId { get; set; }
    public string? LecturerId { get; set; }
    public List<string> LabAssistantIds { get; set; } = new();
    public int LectureHours { get; set; }
    public int LabHours { get; set; }
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CourseDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CourseDto> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.LecturerId))
            throw new Common.Exceptions.ValidationException("A lecturer must be assigned to the course.");

        var course = new Course
        {
            Name = request.Name,
            TrackId = request.TrackId,
            LecturerId = string.IsNullOrEmpty(request.LecturerId) ? null : request.LecturerId,
            LectureHours = request.LectureHours,
            LabHours = request.LabHours,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CourseLabAssistants = request.LabAssistantIds.Select(id => new CourseLabAssistant { InstructorId = id }).ToList()
        };

        await _courseRepository.AddAsync(course, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            TrackId = course.TrackId,
            LecturerId = course.LecturerId,
            // LabAssistants are populated from DB or empty initially for DTO
            LabAssistants = new List<InstructorDto>(),
            LectureHours = course.LectureHours,
            LabHours = course.LabHours,
            NumberOfLectures = course.NumberOfLectures,
            NumberOfLabs = course.NumberOfLabs,
            IsActive = course.IsActive
        };
    }
}
