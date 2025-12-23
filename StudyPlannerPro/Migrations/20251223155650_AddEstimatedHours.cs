using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyPlannerPro.Migrations
{
    /// <inheritdoc />
    public partial class AddEstimatedHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "EstimatedTotalHours",
                table: "Exams",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedTotalHours",
                table: "Exams");
        }
    }
}
