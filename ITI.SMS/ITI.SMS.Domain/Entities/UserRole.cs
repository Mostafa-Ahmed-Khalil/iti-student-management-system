using Microsoft.AspNetCore.Identity;

namespace ITI.SMS.Domain.Entities;

public class UserRole : IdentityUserRole<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual ApplicationRole Role { get; set; } = null!;
}
