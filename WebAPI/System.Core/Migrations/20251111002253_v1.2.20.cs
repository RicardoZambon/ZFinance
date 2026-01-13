using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Niten.System.Core.Migrations
{
    /// <inheritdoc />
    public partial class v1220 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
ALTER TABLE `Configs_GruposPagamentosOnline` 
RENAME TO `PortalAluno_GruposPagamentosOnline`;
");

            migrationBuilder.DropTable(
                name: "Jobs_Logs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Integracoes_Perfis");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Integracoes_Perfis");

            migrationBuilder.AddColumn<bool>(
                name: "Desativado",
                table: "Integracoes_Perfis",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}