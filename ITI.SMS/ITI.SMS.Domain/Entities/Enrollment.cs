namespace ITI.SMS.Domain.Entities;

public class Enrollment
{
    public int Id { get; set; }

    public int TrackId { get; set; }
    public Track Track { get; set; } = null!;

    public string StudentId { get; set; } = string.Empty;
    public ApplicationUser Student { get; set; } = null!;

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}
