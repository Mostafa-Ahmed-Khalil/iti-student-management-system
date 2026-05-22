using ITI.SMS.Application.Common.Exceptions;
using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ITI.SMS.Application.Enrollments.Commands;

public record EnrollStudentsCommand(int TrackId, List<string> StudentIds) : IRequest<Unit>;

public class EnrollStudentsCommandHandler : IRequestHandler<EnrollStudentsCommand, Unit>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public EnrollStudentsCommandHandler(
        IEnrollmentRepository enrollmentRepository,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _enrollmentRepository = enrollmentRepository;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(EnrollStudentsCommand request, CancellationToken cancellationToken)
    {
        if (request.StudentIds == null || request.StudentIds.Count == 0)
        {
            return Unit.Value;
        }

        foreach (var studentId in request.StudentIds)
        {
            var student = await _userManager.FindByIdAsync(studentId)
                ?? throw new ValidationException($"Student with ID {studentId} not found.");

            var isStudent = await _userManager.IsInRoleAsync(student, "Student");
            if (!isStudent)
                throw new ValidationException($"User {student.FullName} does not have the Student role.");

            var existing = await _enrollmentRepository.GetAsync(request.TrackId, studentId, cancellationToken);
            if (existing != null)
            {
                // If it exists, reactivate it if it's inactive
                if (!existing.IsActive)
                {
                    existing.IsActive = true;
                }
                // If already active, we just skip it (no action needed, avoids duplicate)
            }
            else
            {
                var enrollment = new Enrollment
                {
                    TrackId = request.TrackId,
                    StudentId = studentId,
                    IsActive = true
                };

                await _enrollmentRepository.AddAsync(enrollment, cancellationToken);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
