using Microsoft.EntityFrameworkCore.Migrations;

namespace GarderieManagementClean.Infrastructure.Migrations
{
    public partial class FixedNavigPropretyInAppUserTutorEnfant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enfants_AspNetUsers_ApplicationUserId",
                table: "Enfants");

            migrationBuilder.DropIndex(
                name: "IX_Enfants_ApplicationUserId",
                table: "Enfants");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Enfants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Enfants",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enfants_ApplicationUserId",
                table: "Enfants",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enfants_AspNetUsers_ApplicationUserId",
                table: "Enfants",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
