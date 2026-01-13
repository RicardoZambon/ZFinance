using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Niten.System.Core.Migrations
{
    /// <inheritdoc />
    public partial class v1221 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_Financeiro_PagamentosOnline_PortalAluno_GruposPagamentosOnli~",
                table: "Financeiro_PagamentosOnline");

            migrationBuilder.Sql(@"
ALTER TABLE `Financeiro_PagamentosOnline` 
CHANGE COLUMN `GrupoPagamentosOnlineID` `GrupoID` BIGINT NOT NULL DEFAULT '0';
");

            migrationBuilder.RenameIndex(
                name: "IX_Financeiro_PagamentosOnline_GrupoPagamentosOnlineID",
                table: "Financeiro_PagamentosOnline",
                newName: "IX_Financeiro_PagamentosOnline_GrupoID");

            migrationBuilder.Sql(@"
ALTER TABLE `PortalAluno_GruposPagamentosOnline` 
RENAME TO `PortalAluno_Grupos`;
");

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "PortalAluno_Grupos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.RenameIndex(
                name: "IX_Configs_GruposPagamentosOnline_CreatedByID",
                table: "PortalAluno_Grupos",
                newName: "IX_PortalAluno_Grupos_CreatedByID");

            migrationBuilder.RenameIndex(
                name: "IX_Configs_GruposPagamentosOnline_LastChangedByID",
                table: "PortalAluno_Grupos",
                newName: "IX_PortalAluno_Grupos_LastChangedByID");

            migrationBuilder.AddForeignKey(
                name: "FK_Financeiro_PagamentosOnline_PortalAluno_Grupos_GrupoID",
                table: "Financeiro_PagamentosOnline",
                column: "GrupoID",
                principalTable: "PortalAluno_Grupos",
                principalColumn: "ID");

            migrationBuilder.CreateTable(
                name: "PortalAluno_TiposPlanos",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    NomeExibicao = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "longblob", rowVersion: true, nullable: true)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME(0)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LastChangedOn = table.Column<DateTime>(type: "DATETIME(0)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.ComputedColumn),
                    CreatedByID = table.Column<int>(type: "int", nullable: false),
                    LastChangedByID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortalAluno_TiposPlanos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PortalAluno_TiposPlanos_sys_cadastro_CreatedByID",
                        column: x => x.CreatedByID,
                        principalTable: "sys_cadastro",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_PortalAluno_TiposPlanos_sys_cadastro_LastChangedByID",
                        column: x => x.LastChangedByID,
                        principalTable: "sys_cadastro",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "latin1");

            migrationBuilder.CreateIndex(
                name: "IX_PortalAluno_TiposPlanos_CreatedByID",
                table: "PortalAluno_TiposPlanos",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_PortalAluno_TiposPlanos_LastChangedByID",
                table: "PortalAluno_TiposPlanos",
                column: "LastChangedByID");

            migrationBuilder.AddColumn<long>(
                name: "TipoPlanoID",
                table: "Financeiro_Planos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Financeiro_Planos_PortalAluno_TiposPlanos_TipoPlanoID",
                table: "Financeiro_Planos",
                column: "TipoPlanoID",
                principalTable: "PortalAluno_TiposPlanos",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
