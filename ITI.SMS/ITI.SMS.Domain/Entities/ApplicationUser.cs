using Microsoft.AspNetCore.Identity;

namespace ITI.SMS.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    
    // Many-to-many navigation property via the custom UserRole join entity
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
