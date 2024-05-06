using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlyCreateDatabase.Migrations
{
    /// <inheritdoc />
    public partial class FixExerciseCourseNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_Courses_CourseId",
                table: "Exercise");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_Courses_CourseId",
                table: "Exercise");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Exercise",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_Courses_CourseId",
                table: "Exercise",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
