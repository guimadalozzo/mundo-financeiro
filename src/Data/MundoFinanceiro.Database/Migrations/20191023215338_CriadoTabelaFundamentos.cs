using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MundoFinanceiro.Database.Migrations
{
    public partial class CriadoTabelaFundamentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fundamentos",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    papel_id = table.Column<short>(nullable: false),
                    lpa = table.Column<float>(nullable: false),
                    vpa = table.Column<float>(nullable: false),
                    roe = table.Column<float>(nullable: false),
                    roic = table.Column<float>(nullable: false),
                    valor_mercado = table.Column<decimal>(nullable: false),
                    data = table.Column<DateTime>(nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fundamentos", x => x.id);
                    table.ForeignKey(
                        name: "fk_fundamentos_papeis_papel_id",
                        column: x => x.papel_id,
                        principalTable: "papeis",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_fundamentos_papel_id",
                table: "fundamentos",
                column: "papel_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fundamentos");
        }
    }
}
