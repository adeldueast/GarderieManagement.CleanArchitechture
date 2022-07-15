using Microsoft.EntityFrameworkCore.Migrations;

namespace GarderieManagementClean.Infrastructure.Migrations
{
    public partial class AddePropToJournalDeBordEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Participation_Description",
                table: "JournalDeBords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Participation_Rating",
                table: "JournalDeBords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Participation_Description",
                table: "JournalDeBords");

            migrationBuilder.DropColumn(
                name: "Participation_Rating",
                table: "JournalDeBords");
        }
    }
}
