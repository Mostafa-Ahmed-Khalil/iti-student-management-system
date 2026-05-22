using ITI.SMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ITI.SMS.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        // First run any pending migrations to ensure database exists and is up to date
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();

        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        // Seed Roles
        string[] roles = ["Admin", "Branch Manager", "Technical Supervisor", "Student", "Instructor"];
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
            }
        }

        // Seed Default Admin User
        var defaultUserEmail = "admin@iti.edu";
        var defaultUser = await userManager.FindByEmailAsync(defaultUserEmail);
        if (defaultUser == null)
        {
            defaultUser = new ApplicationUser
            {
                UserName = defaultUserEmail,
                Email = defaultUserEmail,
                FullName = "Admin User",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(defaultUser, "AdminPass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(defaultUser, roles);
            }
        }
    }
}
