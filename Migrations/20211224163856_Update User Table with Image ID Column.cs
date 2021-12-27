using Microsoft.EntityFrameworkCore.Migrations;

namespace Rock_Market.Migrations
{
    public partial class UpdateUserTablewithImageIDColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Image",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageID",
                table: "AspNetUsers",
                type: "nvarchar(MAX)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "ProfileImageID",
                table: "AspNetUsers");
        }
    }
}
