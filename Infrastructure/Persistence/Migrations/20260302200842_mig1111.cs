using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig1111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "WorkoutLogs");

            migrationBuilder.AddColumn<int>(
                name: "BaseProgramId",
                table: "WorkoutPrograms",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "WorkoutLogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseProgramId",
                table: "WorkoutPrograms");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WorkoutLogs");

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "WorkoutLogs",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
