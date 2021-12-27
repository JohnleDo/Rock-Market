using Microsoft.EntityFrameworkCore.Migrations;

namespace Rock_Market.Migrations
{
    public partial class UpdateStoreTableAddImageID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageID",
                table: "Store",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageID",
                table: "Store");
        }
    }
}
