using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Enrollments.Queries;

public record StudentDto(string Id, string FullName, string Email);

public record GetEnrolledStudentsQuery(int TrackId) : IRequest<List<StudentDto>>;

public class GetEnrolledStudentsQueryHandler : IRequestHandler<GetEnrolledStudentsQuery, List<StudentDto>>
{
    private readonly IEnrollmentRepository _enrollmentRepository;

    public GetEnrolledStudentsQueryHandler(IEnrollmentRepository enrollmentRepository)
    {
        _enrollmentRepository = enrollmentRepository;
    }

    public async Task<List<StudentDto>> Handle(GetEnrolledStudentsQuery request, CancellationToken cancellationToken)
    {
        var enrollments = await _enrollmentRepository.GetByTrackIdAsync(request.TrackId, cancellationToken);

        return enrollments.Select(e => new StudentDto(
            e.StudentId,
            e.Student.FullName ?? string.Empty,
            e.Student.Email ?? string.Empty
        )).ToList();
    }
}
