using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.SMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLocationFromBranch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "location",
                table: "branches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "location",
                table: "branches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
