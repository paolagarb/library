using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class fotocapa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Livro",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Dados",
                table: "Livro",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Livro");

            migrationBuilder.DropColumn(
                name: "Dados",
                table: "Livro");
        }
    }
}
