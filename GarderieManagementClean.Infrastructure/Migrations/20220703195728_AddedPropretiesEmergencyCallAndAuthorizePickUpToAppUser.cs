using Microsoft.EntityFrameworkCore.Migrations;

namespace GarderieManagementClean.Infrastructure.Migrations
{
    public partial class AddedPropretiesEmergencyCallAndAuthorizePickUpToAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AuthorizePickup",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmergencyContact",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "hasAccount",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorizePickup",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmergencyContact",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "hasAccount",
                table: "AspNetUsers");
        }
    }
}
