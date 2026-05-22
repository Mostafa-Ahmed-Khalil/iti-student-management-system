using ITI.SMS.Application.Tracks.DTOs;
using ITI.SMS.Domain.Entities;
using ITI.SMS.Domain.Interfaces;
using MediatR;

namespace ITI.SMS.Application.Tracks.Queries;

public class GetTracksByBranchQueryHandler : IRequestHandler<GetTracksByBranchQuery, List<TrackDto>>
{
    private readonly ITrackRepository _trackRepository;

    public GetTracksByBranchQueryHandler(ITrackRepository trackRepository)
    {
        _trackRepository = trackRepository;
    }

    public async Task<List<TrackDto>> Handle(GetTracksByBranchQuery request, CancellationToken cancellationToken)
    {
        var tracks = await _trackRepository.GetByBranchIdAsync(request.BranchId, cancellationToken);

        return tracks.Select(t => new TrackDto
        {
            Id = t.Id,
            Name = t.Name,
            StartDate = t.StartDate,
            BranchId = t.BranchId,
            IsActive = t.IsActive,
            SupervisorId = t.SupervisorId ?? string.Empty,
            SupervisorName = t.Supervisor != null ? (t.Supervisor.FullName ?? t.Supervisor.Email) : string.Empty
        }).ToList();
    }
}
