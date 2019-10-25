using Microsoft.EntityFrameworkCore.Migrations;

namespace MundoFinanceiro.Database.Migrations
{
    public partial class CriadoPapeisIniciais : Migration
    {
        private static readonly string[] PapeisInicias = new string[]
        {
            "PETR4",
            "MGLU3",
            "ELPL3",
            "PRIO3",
            "GGBR3",
            "KLBN3",
            "LAME3",
            "BBAS3",
            "ELET3",
            "VVAR3"
        };
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string insertFormat = "INSERT INTO papeis (id, nome) VALUES({0}, '{1}')";

            for (var index = 0; index < PapeisInicias.Length; index++)
                migrationBuilder.Sql(string.Format(insertFormat, index + 1, PapeisInicias[index]));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
