using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Niten.System.Core.Migrations
{
    /// <inheritdoc />
    public partial class v1213 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiaVencimento",
                table: "Cache_PagamentosOnlineRecorrencias",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTemporary",
                table: "Cache_PagamentosOnlineRecorrencias",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaVencimento",
                table: "Cache_PagamentosOnlineRecorrencias");

            migrationBuilder.DropColumn(
                name: "IsTemporary",
                table: "Cache_PagamentosOnlineRecorrencias");
        }
    }
}