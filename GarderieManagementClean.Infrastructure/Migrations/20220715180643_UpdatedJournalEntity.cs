using Microsoft.EntityFrameworkCore.Migrations;

namespace GarderieManagementClean.Infrastructure.Migrations
{
    public partial class UpdatedJournalEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Humeur_Description",
                table: "JournalDeBords");

            migrationBuilder.DropColumn(
                name: "Manger_Description",
                table: "JournalDeBords");

            migrationBuilder.RenameColumn(
                name: "Toilette_Description",
                table: "JournalDeBords",
                newName: "Manger_Message");

            migrationBuilder.RenameColumn(
                name: "Participation_Description",
                table: "JournalDeBords",
                newName: "Commentaire_Message");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "JournalDeBords",
                newName: "Activite_Message");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Manger_Message",
                table: "JournalDeBords",
                newName: "Toilette_Description");

            migrationBuilder.RenameColumn(
                name: "Commentaire_Message",
                table: "JournalDeBords",
                newName: "Participation_Description");

            migrationBuilder.RenameColumn(
                name: "Activite_Message",
                table: "JournalDeBords",
                newName: "Message");

            migrationBuilder.AddColumn<string>(
                name: "Humeur_Description",
                table: "JournalDeBords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manger_Description",
                table: "JournalDeBords",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
