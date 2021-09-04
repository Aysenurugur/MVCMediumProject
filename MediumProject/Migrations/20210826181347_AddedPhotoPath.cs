using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MediumProject.Migrations
{
    public partial class AddedPhotoPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Users");

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Users",
                type: "image",
                nullable: true);
        }
    }
}
