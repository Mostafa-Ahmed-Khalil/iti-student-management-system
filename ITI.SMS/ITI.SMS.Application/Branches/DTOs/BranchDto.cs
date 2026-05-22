namespace ITI.SMS.Application.Branches.DTOs;

public class BranchDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }
    public string? ManagerId { get; set; }
    public string? ManagerName { get; set; }
}
