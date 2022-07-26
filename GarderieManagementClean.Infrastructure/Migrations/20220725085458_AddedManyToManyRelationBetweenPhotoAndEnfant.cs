using Microsoft.EntityFrameworkCore.Migrations;

namespace GarderieManagementClean.Infrastructure.Migrations
{
    public partial class AddedManyToManyRelationBetweenPhotoAndEnfant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Enfants_EnfantId1",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_EnfantId1",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "EnfantId1",
                table: "Photos");

            migrationBuilder.CreateTable(
                name: "EnfantPhoto",
                columns: table => new
                {
                    EnfantsId = table.Column<int>(type: "int", nullable: false),
                    PhotosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnfantPhoto", x => new { x.EnfantsId, x.PhotosId });
                    table.ForeignKey(
                        name: "FK_EnfantPhoto_Enfants_EnfantsId",
                        column: x => x.EnfantsId,
                        principalTable: "Enfants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnfantPhoto_Photos_PhotosId",
                        column: x => x.PhotosId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnfantPhoto_PhotosId",
                table: "EnfantPhoto",
                column: "PhotosId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnfantPhoto");

            migrationBuilder.AddColumn<int>(
                name: "EnfantId1",
                table: "Photos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EnfantId1",
                table: "Photos",
                column: "EnfantId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Enfants_EnfantId1",
                table: "Photos",
                column: "EnfantId1",
                principalTable: "Enfants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
