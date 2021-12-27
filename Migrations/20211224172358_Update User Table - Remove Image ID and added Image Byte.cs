using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Rock_Market.Migrations
{
    public partial class UpdateUserTableRemoveImageIDandaddedImageByte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageID",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "AspNetUsers",
                type: "varbinary(MAX)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageID",
                table: "AspNetUsers",
                type: "nvarchar(MAX)",
                nullable: true);
        }
    }
}
