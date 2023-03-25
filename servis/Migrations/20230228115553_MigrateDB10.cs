using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace servis.Migrations
{
    public partial class MigrateDB10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GetSession",
                columns: table => new
                {
                    Session_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date_Session = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Format_Session = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Psychologist_objId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetSession", x => x.Session_ID);
                    table.ForeignKey(
                        name: "FK_GetSession_Psychologist_Psychologist_objId",
                        column: x => x.Psychologist_objId,
                        principalTable: "Psychologist",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GetSession_Psychologist_objId",
                table: "GetSession",
                column: "Psychologist_objId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GetSession");
        }
    }
}
