﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlyCreateDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddBlobToFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FileBlob",
                table: "Files",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileBlob",
                table: "Files");
        }
    }
}
