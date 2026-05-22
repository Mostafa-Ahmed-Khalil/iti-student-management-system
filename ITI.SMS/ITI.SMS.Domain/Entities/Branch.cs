namespace ITI.SMS.Domain.Entities;

public class Branch
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? ManagerId { get; set; }
    public ApplicationUser? Manager { get; set; }
    
    public ICollection<Track> Tracks { get; set; } = new List<Track>();
}
