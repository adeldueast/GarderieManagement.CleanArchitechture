using Microsoft.EntityFrameworkCore.Migrations;

namespace GarderieManagementClean.Infrastructure.Migrations
{
    public partial class ChangedTutorEnfant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserEnfant");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ApplicationUserEnfant",
                columns: table => new
                {
                    EnfantsId = table.Column<int>(type: "int", nullable: false),
                    TutorsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserEnfant", x => new { x.EnfantsId, x.TutorsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserEnfant_AspNetUsers_TutorsId",
                        column: x => x.TutorsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserEnfant_Enfants_EnfantsId",
                        column: x => x.EnfantsId,
                        principalTable: "Enfants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserEnfant_TutorsId",
                table: "ApplicationUserEnfant",
                column: "TutorsId");
        }
    }
}
