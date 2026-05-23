using ITI.SMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ITI.SMS.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<
    ApplicationUser, 
    ApplicationRole, 
    string, 
    IdentityUserClaim<string>, 
    UserRole, 
    IdentityUserLogin<string>, 
    IdentityRoleClaim<string>, 
    IdentityUserToken<string>>
{
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseLabAssistant> CourseLabAssistants { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<LabEvaluation> LabEvaluations { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure custom many-to-many join relationship for Identity
        builder.Entity<UserRole>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            userRole.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });

        builder.Entity<Branch>()
            .HasOne(b => b.Manager)
            .WithMany()
            .HasForeignKey(b => b.ManagerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Track>()
            .HasOne(t => t.Supervisor)
            .WithMany()
            .HasForeignKey(t => t.SupervisorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Course>()
            .HasOne(c => c.Track)
            .WithMany(t => t.Courses)
            .HasForeignKey(c => c.TrackId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Course>()
            .HasOne(c => c.Lecturer)
            .WithMany()
            .HasForeignKey(c => c.LecturerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<CourseLabAssistant>()
            .HasKey(cla => new { cla.CourseId, cla.InstructorId });

        builder.Entity<CourseLabAssistant>()
            .HasOne(cla => cla.Course)
            .WithMany(c => c.CourseLabAssistants)
            .HasForeignKey(cla => cla.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CourseLabAssistant>()
            .HasOne(cla => cla.Instructor)
            .WithMany()
            .HasForeignKey(cla => cla.InstructorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Enrollment>()
            .HasOne(e => e.Track)
            .WithMany()
            .HasForeignKey(e => e.TrackId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany()
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Enrollment>()
            .HasIndex(e => new { e.TrackId, e.StudentId })
            .IsUnique();

        builder.Entity<LabEvaluation>()
            .HasOne(le => le.Course)
            .WithMany()
            .HasForeignKey(le => le.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<LabEvaluation>()
            .HasOne(le => le.Student)
            .WithMany()
            .HasForeignKey(le => le.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<LabEvaluation>()
            .HasIndex(le => new { le.CourseId, le.StudentId, le.LabNumber })
            .IsUnique();

        builder.Entity<LabEvaluation>()
            .HasOne(le => le.Evaluator)
            .WithMany()
            .HasForeignKey(le => le.EvaluatorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<LabEvaluation>()
            .Property(le => le.Score)
            .HasPrecision(18, 2);

        // Map tables and columns to snake_case
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            // Set snake_case table name (remove AspNet prefix if present)
            var tableName = entity.GetTableName();
            if (tableName != null)
            {
                entity.SetTableName(ConvertToSnakeCase(tableName));
            }

            // Set snake_case column names
            foreach (var property in entity.GetProperties())
            {
                var columnName = property.GetColumnName();
                if (columnName != null)
                {
                    property.SetColumnName(ConvertToSnakeCase(columnName));
                }
            }

            // Set snake_case key names
            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (keyName != null)
                {
                    key.SetName(ConvertToSnakeCase(keyName));
                }
            }

            // Set snake_case foreign key names
            foreach (var fk in entity.GetForeignKeys())
            {
                var fkName = fk.GetConstraintName();
                if (fkName != null)
                {
                    fk.SetConstraintName(ConvertToSnakeCase(fkName));
                }
            }

            // Set snake_case index names
            foreach (var index in entity.GetIndexes())
            {
                var indexName = index.GetDatabaseName();
                if (indexName != null)
                {
                    index.SetDatabaseName(ConvertToSnakeCase(indexName));
                }
            }
        }
    }

    private static string ConvertToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        // Strip standard ASP.NET Identity prefixes for cleaner tables
        if (input.StartsWith("AspNet"))
        {
            input = input.Substring(6);
        }

        // Add underscore before uppercase letters (excluding the first letter)
        var result = Regex.Replace(input, @"(?<!^)(?=[A-Z])", "_");
        return result.ToLower();
    }
}
