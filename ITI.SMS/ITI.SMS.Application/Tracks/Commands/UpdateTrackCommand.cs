using MediatR;

namespace ITI.SMS.Application.Tracks.Commands;

public class UpdateTrackCommand : IRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public bool IsActive { get; set; }
    public string SupervisorId { get; set; } = string.Empty;

    public UpdateTrackCommand(int id, string name, DateTime startDate, bool isActive, string supervisorId)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        IsActive = isActive;
        SupervisorId = supervisorId;
    }
}
