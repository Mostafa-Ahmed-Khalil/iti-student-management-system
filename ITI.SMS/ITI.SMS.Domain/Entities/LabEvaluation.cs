namespace ITI.SMS.Domain.Entities;

public class LabEvaluation
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public string StudentId { get; set; } = string.Empty;
    public ApplicationUser Student { get; set; } = null!;

    /// <summary>1-indexed lab number (Lab 1, Lab 2, ...)</summary>
    public int LabNumber { get; set; }

    /// <summary>Score: 0 to 10</summary>
    public decimal Score { get; set; }

    /// <summary>Visible to the student on their dashboard</summary>
    public string TechNotes { get; set; } = string.Empty;

    /// <summary>Instructor-only — never served to Student role</summary>
    public string SoftSkillsNotes { get; set; } = string.Empty;

    public string EvaluatorId { get; set; } = string.Empty;
    public ApplicationUser Evaluator { get; set; } = null!;

    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
}
