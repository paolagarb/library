using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class fkuserlivros : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Livro",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Livro_IdentityUserId",
                table: "Livro",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Livro_AspNetUsers_IdentityUserId",
                table: "Livro",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livro_AspNetUsers_IdentityUserId",
                table: "Livro");

            migrationBuilder.DropIndex(
                name: "IX_Livro_IdentityUserId",
                table: "Livro");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Livro");
        }
    }
}
