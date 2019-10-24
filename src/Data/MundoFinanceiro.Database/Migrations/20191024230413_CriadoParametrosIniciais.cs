using Microsoft.EntityFrameworkCore.Migrations;

namespace MundoFinanceiro.Database.Migrations
{
    public partial class CriadoParametrosIniciais : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string insertFormat = "INSERT INTO parametros (id, chave, descricao, valor_texto, valor_numerico) VALUES ({0}, '{1}', '{2}', '{3}', {4})";
            migrationBuilder.Sql(
                string.Format(insertFormat,
                    1,
                    "MAX_NUM_PROCESSAMENTO_PENDENTES",
                    "Número máximo de threads no processamento dos papéis pendentes",
                    null,
                    4));
            
            migrationBuilder.Sql(
                string.Format(insertFormat,
                    2,
                    "INTERVALO_PROCESSAMENTO_PENDENTES",
                    "Intervalo de processamento no multitasking no processamento dos papéis pendentes",
                    null,
                    10));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM parametros p WHERE p.id IN (1, 2)");
        }
    }
}
