using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Instructor.Queries;

public class InstructorCourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TrackId { get; set; }
    public string TrackName { get; set; } = string.Empty;
    public int NumberOfLabs { get; set; }
    public int NumberOfLectures { get; set; }
    public int LabHours { get; set; }
    public int LectureHours { get; set; }
    public string Role { get; set; } = string.Empty;
}

public record GetMyCoursesQuery(string InstructorId) : IRequest<List<InstructorCourseDto>>;

public class GetMyCoursesQueryHandler : IRequestHandler<GetMyCoursesQuery, List<InstructorCourseDto>>
{
    private readonly ICourseRepository _courseRepository;

    public GetMyCoursesQueryHandler(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<List<InstructorCourseDto>> Handle(GetMyCoursesQuery request, CancellationToken cancellationToken)
    {
        var courses = await _courseRepository.GetByInstructorIdAsync(request.InstructorId, cancellationToken);

        return courses.Select(c => new InstructorCourseDto
        {
            Id = c.Id,
            Name = c.Name,
            TrackId = c.TrackId,
            TrackName = c.Track?.Name ?? string.Empty,
            NumberOfLabs = c.NumberOfLabs,
            NumberOfLectures = c.NumberOfLectures,
            LabHours = c.LabHours,
            LectureHours = c.LectureHours,
            Role = (c.LecturerId == request.InstructorId && c.CourseLabAssistants.Any(la => la.InstructorId == request.InstructorId)) 
                ? "Both" 
                : (c.LecturerId == request.InstructorId ? "Lecturer" : "Lab Assistant")
        }).ToList();
    }
}
