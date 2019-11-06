using Microsoft.EntityFrameworkCore.Migrations;

namespace MundoFinanceiro.Database.Migrations
{
    public partial class DesativadoPapeisNaoFuncionaisOnvest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE papeis
                                      SET ativo = false
                                    WHERE nome NOT IN ('PRIO3', 'MGLU3', 'GGBR3', 'PETR4', 'VVAR3', 'BBAS3', 'ELPL3', 'KLBN3', 'LAME3');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
