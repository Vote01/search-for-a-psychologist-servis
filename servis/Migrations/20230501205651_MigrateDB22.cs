using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace servis.Migrations
{
    public partial class MigrateDB22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientID",
                table: "GetSession",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GetSession_ClientID",
                table: "GetSession",
                column: "ClientID");

            migrationBuilder.AddForeignKey(
                name: "FK_GetSession_Client_ClientID",
                table: "GetSession",
                column: "ClientID",
                principalTable: "Client",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetSession_Client_ClientID",
                table: "GetSession");

            migrationBuilder.DropIndex(
                name: "IX_GetSession_ClientID",
                table: "GetSession");

            migrationBuilder.DropColumn(
                name: "ClientID",
                table: "GetSession");
        }
    }
}
