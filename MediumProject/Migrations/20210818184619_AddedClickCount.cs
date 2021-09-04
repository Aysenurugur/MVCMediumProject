using Microsoft.EntityFrameworkCore.Migrations;

namespace MediumProject.Migrations
{
    public partial class AddedClickCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "ClickCount",
                table: "Stories",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StoryID",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "ClickCount",
                table: "Stories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoryID",
                table: "Stories",
                column: "StoryID");
        }
    }
}
