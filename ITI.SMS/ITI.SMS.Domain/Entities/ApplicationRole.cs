using Microsoft.AspNetCore.Identity;

namespace ITI.SMS.Domain.Entities;

public class ApplicationRole : IdentityRole
{
    // Navigation property for many-to-many relationship
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
