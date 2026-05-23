using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Instructor.Queries;

public class LabEvaluationDto
{
    public int Id { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public int LabNumber { get; set; }
    public decimal Score { get; set; }
    public string TechNotes { get; set; } = string.Empty;
    public string? SoftSkillsNotes { get; set; }  // null when caller is Student
    public string? EvaluatorId { get; set; }
    public string? EvaluatorName { get; set; }
}

public class LabEvaluationGridDto
{
    public int CourseId { get; set; }
    public int NumberOfLabs { get; set; }
    public List<StudentDto2> Students { get; set; } = new();
    public List<LabEvaluationDto> Evaluations { get; set; } = new();
}

public record StudentDto2(string Id, string FullName, string Email);

public record GetLabEvaluationsQuery(int CourseId, int TrackId, bool IsInstructor) : IRequest<LabEvaluationGridDto>;

public class GetLabEvaluationsQueryHandler : IRequestHandler<GetLabEvaluationsQuery, LabEvaluationGridDto>
{
    private readonly ILabEvaluationRepository _labEvalRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICourseRepository _courseRepository;

    public GetLabEvaluationsQueryHandler(
        ILabEvaluationRepository labEvalRepository,
        IEnrollmentRepository enrollmentRepository,
        ICourseRepository courseRepository)
    {
        _labEvalRepository = labEvalRepository;
        _enrollmentRepository = enrollmentRepository;
        _courseRepository = courseRepository;
    }

    public async Task<LabEvaluationGridDto> Handle(GetLabEvaluationsQuery request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken)
            ?? throw new KeyNotFoundException("Course not found.");

        var enrollments = await _enrollmentRepository.GetByTrackIdAsync(request.TrackId, cancellationToken);
        var evaluations = await _labEvalRepository.GetByCourseAsync(request.CourseId, cancellationToken);

        return new LabEvaluationGridDto
        {
            CourseId = request.CourseId,
            NumberOfLabs = course.NumberOfLabs,
            Students = enrollments.Select(e => new StudentDto2(
                e.StudentId,
                e.Student.FullName ?? string.Empty,
                e.Student.Email ?? string.Empty
            )).ToList(),
            Evaluations = evaluations.Select(le => new LabEvaluationDto
            {
                Id = le.Id,
                StudentId = le.StudentId,
                StudentName = le.Student.FullName ?? string.Empty,
                LabNumber = le.LabNumber,
                Score = le.Score,
                TechNotes = le.TechNotes,
                // Strip soft skills notes if caller is not an Instructor
                SoftSkillsNotes = request.IsInstructor ? le.SoftSkillsNotes : null,
                EvaluatorId = request.IsInstructor ? le.EvaluatorId : null,
                EvaluatorName = request.IsInstructor ? (le.Evaluator != null ? (le.Evaluator.FullName ?? le.Evaluator.Email) : null) : null
            }).ToList()
        };
    }
}
