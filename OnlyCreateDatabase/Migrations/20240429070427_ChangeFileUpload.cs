using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlyCreateDatabase.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFileUpload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileNameDatabase",
                table: "Files",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileNameDatabase",
                table: "Files");
        }
    }
}
