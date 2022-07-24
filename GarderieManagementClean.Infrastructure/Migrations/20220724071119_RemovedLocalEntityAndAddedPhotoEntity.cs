using Microsoft.EntityFrameworkCore.Migrations;

namespace GarderieManagementClean.Infrastructure.Migrations
{
    public partial class RemovedLocalEntityAndAddedPhotoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enfants_Locals_LocalId",
                table: "Enfants");

            migrationBuilder.DropTable(
                name: "Locals");

            migrationBuilder.DropIndex(
                name: "IX_Enfants_LocalId",
                table: "Enfants");

            migrationBuilder.DropColumn(
                name: "LocalId",
                table: "Enfants");

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnfantId = table.Column<int>(type: "int", nullable: true),
                    EnfantId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Enfants_EnfantId",
                        column: x => x.EnfantId,
                        principalTable: "Enfants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Photos_Enfants_EnfantId1",
                        column: x => x.EnfantId1,
                        principalTable: "Enfants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EnfantId",
                table: "Photos",
                column: "EnfantId",
                unique: true,
                filter: "[EnfantId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_EnfantId1",
                table: "Photos",
                column: "EnfantId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.AddColumn<int>(
                name: "LocalId",
                table: "Enfants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Locals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locals_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enfants_LocalId",
                table: "Enfants",
                column: "LocalId");

            migrationBuilder.CreateIndex(
                name: "IX_Locals_GroupId",
                table: "Locals",
                column: "GroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Enfants_Locals_LocalId",
                table: "Enfants",
                column: "LocalId",
                principalTable: "Locals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
