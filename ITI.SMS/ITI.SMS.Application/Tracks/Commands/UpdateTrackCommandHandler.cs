using ITI.SMS.Domain.Interfaces;
using ITI.SMS.Domain.Entities;
using MediatR;

namespace ITI.SMS.Application.Tracks.Commands;

public class UpdateTrackCommandHandler : IRequestHandler<UpdateTrackCommand>
{
    private readonly ITrackRepository _trackRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTrackCommandHandler(ITrackRepository trackRepository, IUnitOfWork unitOfWork)
    {
        _trackRepository = trackRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTrackCommand request, CancellationToken cancellationToken)
    {
        var track = await _trackRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (track == null)
            throw new Common.Exceptions.NotFoundException($"Track with ID {request.Id} not found.");

        track.Name = request.Name;
        track.StartDate = request.StartDate;
        track.IsActive = request.IsActive;
        track.SupervisorId = string.IsNullOrEmpty(request.SupervisorId) ? null : request.SupervisorId;
        track.UpdatedAt = DateTime.UtcNow;

        await _trackRepository.UpdateAsync(track, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
