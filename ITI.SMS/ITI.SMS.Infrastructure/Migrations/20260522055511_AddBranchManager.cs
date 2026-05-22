using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.SMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "manager_id",
                table: "branches",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "i_x_branches_manager_id",
                table: "branches",
                column: "manager_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_branches_users_manager_id",
                table: "branches",
                column: "manager_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_branches_users_manager_id",
                table: "branches");

            migrationBuilder.DropIndex(
                name: "i_x_branches_manager_id",
                table: "branches");

            migrationBuilder.DropColumn(
                name: "manager_id",
                table: "branches");
        }
    }
}
