using Microsoft.EntityFrameworkCore.Migrations;

namespace GarderieManagementClean.Infrastructure.Migrations
{
    public partial class AddedDataIdToNotificationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataId",
                table: "Notifications");
        }
    }
}
