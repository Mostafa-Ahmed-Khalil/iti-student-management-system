using MediatR;

namespace ITI.SMS.Application.Tracks.Commands;

public class DeleteTrackCommand : IRequest
{
    public int Id { get; set; }

    public DeleteTrackCommand(int id)
    {
        Id = id;
    }
}
