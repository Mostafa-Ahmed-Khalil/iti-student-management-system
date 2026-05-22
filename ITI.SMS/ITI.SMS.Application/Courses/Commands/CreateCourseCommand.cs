using ITI.SMS.Application.Courses.DTOs;
using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Courses.Commands;

public class CreateCourseCommand : IRequest<CourseDto>
{
    public string Name { get; set; } = string.Empty;
    public int TrackId { get; set; }
    public string? InstructorId { get; set; }
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
        var course = new Course
        {
            Name = request.Name,
            TrackId = request.TrackId,
            InstructorId = string.IsNullOrEmpty(request.InstructorId) ? null : request.InstructorId,
            LectureHours = request.LectureHours,
            LabHours = request.LabHours,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _courseRepository.AddAsync(course, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            TrackId = course.TrackId,
            InstructorId = course.InstructorId,
            LectureHours = course.LectureHours,
            LabHours = course.LabHours,
            NumberOfLectures = course.NumberOfLectures,
            NumberOfLabs = course.NumberOfLabs,
            IsActive = course.IsActive
        };
    }
}
