using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyPlannerPro.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletedHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CompletedHours",
                table: "Exams",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedHours",
                table: "Exams");
        }
    }
}
