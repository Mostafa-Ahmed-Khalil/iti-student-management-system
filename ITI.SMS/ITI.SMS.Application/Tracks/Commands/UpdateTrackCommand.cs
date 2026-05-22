using MediatR;

namespace ITI.SMS.Application.Tracks.Commands;

public class UpdateTrackCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public bool IsActive { get; set; }

    public UpdateTrackCommand(int id, string name, DateTime startDate, bool isActive)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        IsActive = isActive;
    }
}
