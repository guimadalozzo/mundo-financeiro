using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MundoFinanceiro.Database.Migrations
{
    public partial class CriadoTabelaParametros : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "parametros",
                columns: table => new
                {
                    id = table.Column<short>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    chave = table.Column<string>(maxLength: 64, nullable: false),
                    descricao = table.Column<string>(maxLength: 128, nullable: false),
                    valor_texto = table.Column<string>(nullable: true),
                    valor_numerico = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_parametros", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_parametros_chave",
                table: "parametros",
                column: "chave",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parametros");
        }
    }
}
