using ITI.SMS.Application.Common.Exceptions;
using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Enrollments.Commands;

public record UnenrollStudentCommand(int TrackId, string StudentId) : IRequest<Unit>;

public class UnenrollStudentCommandHandler : IRequestHandler<UnenrollStudentCommand, Unit>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnenrollStudentCommandHandler(
        IEnrollmentRepository enrollmentRepository,
        IUnitOfWork unitOfWork)
    {
        _enrollmentRepository = enrollmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UnenrollStudentCommand request, CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentRepository.GetAsync(request.TrackId, request.StudentId, cancellationToken)
            ?? throw new NotFoundException($"Enrollment for Track {request.TrackId} and Student {request.StudentId} was not found.");

        if (!enrollment.IsActive)
        {
            return Unit.Value; // Already unenrolled
        }

        await _enrollmentRepository.DeleteAsync(enrollment, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
