using Microsoft.EntityFrameworkCore.Migrations;

namespace GarderieManagementClean.Infrastructure.Migrations
{
    public partial class AddedLocalRelationEnfantAndGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocalId",
                table: "Enfants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Local",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Local", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Local_Groups_GroupId",
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
                name: "IX_Local_GroupId",
                table: "Local",
                column: "GroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Enfants_Local_LocalId",
                table: "Enfants",
                column: "LocalId",
                principalTable: "Local",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enfants_Local_LocalId",
                table: "Enfants");

            migrationBuilder.DropTable(
                name: "Local");

            migrationBuilder.DropIndex(
                name: "IX_Enfants_LocalId",
                table: "Enfants");

            migrationBuilder.DropColumn(
                name: "LocalId",
                table: "Enfants");
        }
    }
}
