using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.SMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackSupervisor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "supervisor_id",
                table: "tracks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "i_x_tracks_supervisor_id",
                table: "tracks",
                column: "supervisor_id");

            migrationBuilder.AddForeignKey(
                name: "f_k_tracks_users_supervisor_id",
                table: "tracks",
                column: "supervisor_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_tracks_users_supervisor_id",
                table: "tracks");

            migrationBuilder.DropIndex(
                name: "i_x_tracks_supervisor_id",
                table: "tracks");

            migrationBuilder.DropColumn(
                name: "supervisor_id",
                table: "tracks");
        }
    }
}
