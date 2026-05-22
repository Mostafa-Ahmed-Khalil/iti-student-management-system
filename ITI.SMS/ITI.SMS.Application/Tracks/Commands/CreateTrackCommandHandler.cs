using ITI.SMS.Application.Tracks.DTOs;
using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Tracks.Commands;

public class CreateTrackCommandHandler : IRequestHandler<CreateTrackCommand, TrackDto>
{
    private readonly ITrackRepository _trackRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTrackCommandHandler(ITrackRepository trackRepository, IUnitOfWork unitOfWork)
    {
        _trackRepository = trackRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TrackDto> Handle(CreateTrackCommand request, CancellationToken cancellationToken)
    {
        var track = new Track
        {
            Name = request.Name,
            StartDate = request.StartDate,
            BranchId = request.BranchId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _trackRepository.AddAsync(track, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new TrackDto
        {
            Id = track.Id,
            Name = track.Name,
            StartDate = track.StartDate,
            BranchId = track.BranchId,
            IsActive = track.IsActive,
            CreatedAt = track.CreatedAt
        };
    }
}
