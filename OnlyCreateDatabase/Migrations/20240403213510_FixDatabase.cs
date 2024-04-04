using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlyCreateDatabase.Migrations
{
    /// <inheritdoc />
    public partial class FixDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "createdAt",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "updatedAt",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Grades",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "createdAt",
                table: "Exercise",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "updatedAt",
                table: "Exercise",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "createdAt",
                table: "Courses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "updatedAt",
                table: "Courses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "updatedAt",
                table: "Courses");
        }
    }
}
