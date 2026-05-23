namespace ITI.SMS.Domain.Entities;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public int TrackId { get; set; }
    public Track Track { get; set; } = null!;
    
    public string? LecturerId { get; set; }
    public ApplicationUser? Lecturer { get; set; }
    
    public virtual ICollection<CourseLabAssistant> CourseLabAssistants { get; set; } = new List<CourseLabAssistant>();
    
    public int LectureHours { get; set; }
    public int LabHours { get; set; }
    
    // Calculated fields based on requirements (3 hours = 1 lecture/lab)
    public int NumberOfLectures => LectureHours / 3;
    public int NumberOfLabs => LabHours / 3;
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
