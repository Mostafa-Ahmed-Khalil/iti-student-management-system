namespace ITI.SMS.Domain.Entities;

public class CourseLabAssistant
{
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public string InstructorId { get; set; } = string.Empty;
    public ApplicationUser Instructor { get; set; } = null!;
}
