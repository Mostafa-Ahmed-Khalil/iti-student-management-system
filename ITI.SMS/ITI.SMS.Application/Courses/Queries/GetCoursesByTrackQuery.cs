using ITI.SMS.Application.Courses.DTOs;
using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Courses.Queries;

public record GetCoursesByTrackQuery(int TrackId) : IRequest<List<CourseDto>>;

public class GetCoursesByTrackQueryHandler : IRequestHandler<GetCoursesByTrackQuery, List<CourseDto>>
{
    private readonly ICourseRepository _courseRepository;

    public GetCoursesByTrackQueryHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<List<CourseDto>> Handle(GetCoursesByTrackQuery request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetByTrackIdAsync(request.TrackId, cancellationToken);

        return courses.Select(c => new CourseDto
        {
            Id = c.Id,
            Name = c.Name,
            TrackId = c.TrackId,
            LecturerId = c.LecturerId,
            LecturerName = c.Lecturer != null ? (c.Lecturer.FullName ?? c.Lecturer.Email) : null,
            LabAssistants = c.CourseLabAssistants.Select(la => new InstructorDto
            {
                Id = la.InstructorId,
                Name = la.Instructor.FullName ?? la.Instructor.Email ?? string.Empty
            }).ToList(),
            LectureHours = c.LectureHours,
            LabHours = c.LabHours,
            NumberOfLectures = c.NumberOfLectures,
            NumberOfLabs = c.NumberOfLabs,
            IsActive = c.IsActive
        }).ToList();
    }
}
