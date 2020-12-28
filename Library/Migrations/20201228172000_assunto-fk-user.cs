using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class assuntofkuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Assunto",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assunto_IdentityUserId",
                table: "Assunto",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assunto_AspNetUsers_IdentityUserId",
                table: "Assunto",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assunto_AspNetUsers_IdentityUserId",
                table: "Assunto");

            migrationBuilder.DropIndex(
                name: "IX_Assunto_IdentityUserId",
                table: "Assunto");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Assunto");
        }
    }
}
