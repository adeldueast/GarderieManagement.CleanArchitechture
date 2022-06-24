using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GarderieManagementClean.Infrastructure.Migrations
{
    public partial class AddedJournalDeBord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EducatriceId",
                table: "Groups",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JournalDeBords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Humeur_Rating = table.Column<int>(type: "int", nullable: false),
                    Humeur_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Toilette_Rating = table.Column<int>(type: "int", nullable: false),
                    Toilette_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manger_Rating = table.Column<int>(type: "int", nullable: false),
                    Manger_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnfantId = table.Column<int>(type: "int", nullable: false),
                    EducatriceId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalDeBords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalDeBords_AspNetUsers_EducatriceId",
                        column: x => x.EducatriceId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalDeBords_Enfants_EnfantId",
                        column: x => x.EnfantId,
                        principalTable: "Enfants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_EducatriceId",
                table: "Groups",
                column: "EducatriceId",
                unique: true,
                filter: "[EducatriceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JournalDeBords_EducatriceId",
                table: "JournalDeBords",
                column: "EducatriceId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalDeBords_EnfantId",
                table: "JournalDeBords",
                column: "EnfantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_EducatriceId",
                table: "Groups",
                column: "EducatriceId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_EducatriceId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "JournalDeBords");

            migrationBuilder.DropIndex(
                name: "IX_Groups_EducatriceId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "EducatriceId",
                table: "Groups");
        }
    }
}
