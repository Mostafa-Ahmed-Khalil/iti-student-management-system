namespace ITI.SMS.Application.Tracks.DTOs;

public class TrackDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public int BranchId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
