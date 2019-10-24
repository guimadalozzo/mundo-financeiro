using Microsoft.EntityFrameworkCore.Migrations;

namespace MundoFinanceiro.Database.Migrations
{
    public partial class AlteradoTipoColunasFundamentos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "vpa",
                table: "fundamentos",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "roic",
                table: "fundamentos",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "roe",
                table: "fundamentos",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "lpa",
                table: "fundamentos",
                nullable: false,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "vpa",
                table: "fundamentos",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "roic",
                table: "fundamentos",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "roe",
                table: "fundamentos",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "lpa",
                table: "fundamentos",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
