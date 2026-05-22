using ITI.SMS.Application.Tracks.DTOs;
using MediatR;

namespace ITI.SMS.Application.Tracks.Commands;

public class CreateTrackCommand : IRequest<TrackDto>
{
    public int BranchId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
}
