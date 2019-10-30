using Microsoft.EntityFrameworkCore.Migrations;

namespace MundoFinanceiro.Database.Migrations
{
    public partial class CriadoTabelaReplicacoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "replicacoes",
                columns: table => new
                {
                    id = table.Column<byte>(nullable: false),
                    url = table.Column<string>(maxLength: 64, nullable: false),
                    descricao = table.Column<string>(maxLength: 64, nullable: false),
                    ativo = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_replicacoes", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_replicacoes_url",
                table: "replicacoes",
                column: "url",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "replicacoes");
        }
    }
}
