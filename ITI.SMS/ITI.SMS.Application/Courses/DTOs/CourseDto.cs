namespace ITI.SMS.Application.Courses.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TrackId { get; set; }
    public string? LecturerId { get; set; }
    public string? LecturerName { get; set; }
    public List<InstructorDto> LabAssistants { get; set; } = new();
    
    public int LectureHours { get; set; }
    public int LabHours { get; set; }
    public int NumberOfLectures { get; set; }
    public int NumberOfLabs { get; set; }
    
    public bool IsActive { get; set; }
}

public class InstructorDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
