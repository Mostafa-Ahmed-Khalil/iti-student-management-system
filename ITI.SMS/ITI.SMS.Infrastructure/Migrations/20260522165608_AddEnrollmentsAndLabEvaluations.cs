using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI.SMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrollmentsAndLabEvaluations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "enrollments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    track_id = table.Column<int>(type: "int", nullable: false),
                    student_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    enrolled_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_enrollments", x => x.id);
                    table.ForeignKey(
                        name: "f_k_enrollments__tracks_track_id",
                        column: x => x.track_id,
                        principalTable: "tracks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_enrollments_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lab_evaluations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    course_id = table.Column<int>(type: "int", nullable: false),
                    student_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    lab_number = table.Column<int>(type: "int", nullable: false),
                    score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    tech_notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    soft_skills_notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_lab_evaluations", x => x.id);
                    table.ForeignKey(
                        name: "f_k_lab_evaluations_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "f_k_lab_evaluations_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_enrollments_student_id",
                table: "enrollments",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "i_x_enrollments_track_id_student_id",
                table: "enrollments",
                columns: new[] { "track_id", "student_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_lab_evaluations_course_id_student_id_lab_number",
                table: "lab_evaluations",
                columns: new[] { "course_id", "student_id", "lab_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_lab_evaluations_student_id",
                table: "lab_evaluations",
                column: "student_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enrollments");

            migrationBuilder.DropTable(
                name: "lab_evaluations");
        }
    }
}
