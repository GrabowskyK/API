using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlyCreateDatabase.Migrations
{
    /// <inheritdoc />
    public partial class FixEnrollmentBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_Courses_CourseId",
                table: "Exercise");

            migrationBuilder.RenameColumn(
                name: "IsInCourse",
                table: "Enrollments",
                newName: "UserDecision");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Exercise",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AdminDecision",
                table: "Enrollments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_Courses_CourseId",
                table: "Exercise",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_Courses_CourseId",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "AdminDecision",
                table: "Enrollments");

            migrationBuilder.RenameColumn(
                name: "UserDecision",
                table: "Enrollments",
                newName: "IsInCourse");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Exercise",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_Courses_CourseId",
                table: "Exercise",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
