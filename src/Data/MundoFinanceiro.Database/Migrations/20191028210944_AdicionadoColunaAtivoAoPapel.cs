using Microsoft.EntityFrameworkCore.Migrations;

namespace MundoFinanceiro.Database.Migrations
{
    public partial class AdicionadoColunaAtivoAoPapel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ativo",
                table: "papeis",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ativo",
                table: "papeis");
        }
    }
}
