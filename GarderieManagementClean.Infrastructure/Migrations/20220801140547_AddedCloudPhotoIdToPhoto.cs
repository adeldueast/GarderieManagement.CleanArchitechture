using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarderieManagementClean.Infrastructure.Migrations
{
    public partial class AddedCloudPhotoIdToPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "cloudId",
                table: "Photos",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cloudId",
                table: "Photos");
        }
    }
}
