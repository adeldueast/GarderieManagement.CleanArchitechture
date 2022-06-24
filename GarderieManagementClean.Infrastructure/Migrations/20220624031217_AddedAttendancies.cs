using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GarderieManagementClean.Infrastructure.Migrations
{
    public partial class AddedAttendancies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enfants_Local_LocalId",
                table: "Enfants");

            migrationBuilder.DropForeignKey(
                name: "FK_Local_Groups_GroupId",
                table: "Local");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Local",
                table: "Local");

            migrationBuilder.RenameTable(
                name: "Local",
                newName: "Locals");

            migrationBuilder.RenameIndex(
                name: "IX_Local_GroupId",
                table: "Locals",
                newName: "IX_Locals_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locals",
                table: "Locals",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArrivedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LeftAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnfantId = table.Column<int>(type: "int", nullable: false),
                    AbsenceDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_Enfants_EnfantId",
                        column: x => x.EnfantId,
                        principalTable: "Enfants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EnfantId",
                table: "Attendances",
                column: "EnfantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enfants_Locals_LocalId",
                table: "Enfants",
                column: "LocalId",
                principalTable: "Locals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Locals_Groups_GroupId",
                table: "Locals",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enfants_Locals_LocalId",
                table: "Enfants");

            migrationBuilder.DropForeignKey(
                name: "FK_Locals_Groups_GroupId",
                table: "Locals");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locals",
                table: "Locals");

            migrationBuilder.RenameTable(
                name: "Locals",
                newName: "Local");

            migrationBuilder.RenameIndex(
                name: "IX_Locals_GroupId",
                table: "Local",
                newName: "IX_Local_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Local",
                table: "Local",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enfants_Local_LocalId",
                table: "Enfants",
                column: "LocalId",
                principalTable: "Local",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Local_Groups_GroupId",
                table: "Local",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
