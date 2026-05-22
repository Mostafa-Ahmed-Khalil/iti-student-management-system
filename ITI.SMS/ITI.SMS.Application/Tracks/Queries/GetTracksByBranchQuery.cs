using ITI.SMS.Application.Tracks.DTOs;
using MediatR;

namespace ITI.SMS.Application.Tracks.Queries;

public class GetTracksByBranchQuery : IRequest<List<TrackDto>>
{
    public int BranchId { get; set; }

    public GetTracksByBranchQuery(int branchId)
    {
        BranchId = branchId;
    }
}
