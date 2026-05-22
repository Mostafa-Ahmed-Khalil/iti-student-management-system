using ITI.SMS.Application.Common.Exceptions;
using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Instructor.Commands;

public class UpsertLabEvaluationCommand : IRequest<Unit>
{
    public int CourseId { get; set; }
    public string StudentId { get; set; } = string.Empty;
    public int LabNumber { get; set; }
    public decimal Score { get; set; }
    public string TechNotes { get; set; } = string.Empty;
    public string SoftSkillsNotes { get; set; } = string.Empty;
}

public class UpsertLabEvaluationCommandHandler : IRequestHandler<UpsertLabEvaluationCommand, Unit>
{
    private readonly ILabEvaluationRepository _labEvalRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpsertLabEvaluationCommandHandler(ILabEvaluationRepository labEvalRepository, IUnitOfWork unitOfWork)
    {
        _labEvalRepository = labEvalRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpsertLabEvaluationCommand request, CancellationToken cancellationToken)
    {
        if (request.Score != 0 && request.Score != 1)
            throw new ValidationException("Score must be 0 or 1.");

        if (string.IsNullOrWhiteSpace(request.TechNotes))
            throw new ValidationException("Technical notes are required.");

        if (string.IsNullOrWhiteSpace(request.SoftSkillsNotes))
            throw new ValidationException("Soft skills notes are required.");

        var existing = await _labEvalRepository.GetAsync(request.CourseId, request.StudentId, request.LabNumber, cancellationToken);

        if (existing != null)
        {
            existing.Score = request.Score;
            existing.TechNotes = request.TechNotes;
            existing.SoftSkillsNotes = request.SoftSkillsNotes;
            await _labEvalRepository.UpdateAsync(existing, cancellationToken);
        }
        else
        {
            var evaluation = new LabEvaluation
            {
                CourseId = request.CourseId,
                StudentId = request.StudentId,
                LabNumber = request.LabNumber,
                Score = request.Score,
                TechNotes = request.TechNotes,
                SoftSkillsNotes = request.SoftSkillsNotes
            };
            await _labEvalRepository.AddAsync(evaluation, cancellationToken);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
