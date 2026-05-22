namespace ITI.SMS.Domain.Entities;

public class Track
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    
    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;
    
    public string? SupervisorId { get; set; }
    public ApplicationUser? Supervisor { get; set; }
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
