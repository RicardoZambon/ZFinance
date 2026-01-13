using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZFinance.Core.Migrations
{
    /// <inheritdoc />
    public partial class v100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.EnsureSchema(
                name: "Audit");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Security",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "VARCHAR(250)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByID = table.Column<long>(type: "bigint", nullable: false),
                    LastChangedByID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedByID",
                        column: x => x.CreatedByID,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Users_Users_LastChangedByID",
                        column: x => x.LastChangedByID,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Actions",
                schema: "Security",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "VARCHAR(150)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Entity = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(150)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByID = table.Column<long>(type: "bigint", nullable: false),
                    LastChangedByID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Actions_Users_CreatedByID",
                        column: x => x.CreatedByID,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Actions_Users_LastChangedByID",
                        column: x => x.LastChangedByID,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                schema: "Security",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Icon = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    Label = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ParentMenuID = table.Column<long>(type: "bigint", nullable: true),
                    URL = table.Column<string>(type: "VARCHAR(200)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByID = table.Column<long>(type: "bigint", nullable: false),
                    LastChangedByID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_ParentMenuID",
                        column: x => x.ParentMenuID,
                        principalSchema: "Security",
                        principalTable: "Menus",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Menus_Users_CreatedByID",
                        column: x => x.CreatedByID,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Menus_Users_LastChangedByID",
                        column: x => x.LastChangedByID,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "Security",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Token = table.Column<string>(type: "VARCHAR(50)", nullable: true),
                    UserID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserID",
                        column: x => x.UserID,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Security",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(200)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByID = table.Column<long>(type: "bigint", nullable: false),
                    LastChangedByID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Roles_Users_CreatedByID",
                        column: x => x.CreatedByID,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Roles_Users_LastChangedByID",
                        column: x => x.LastChangedByID,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ServicesHistory",
                schema: "Audit",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChangedOn = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ChangedByID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ServicesHistory_Users_ChangedByID",
                        column: x => x.ChangedByID,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RolesActions",
                schema: "Security",
                columns: table => new
                {
                    ActionID = table.Column<long>(type: "bigint", nullable: false),
                    RoleID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesActions", x => new { x.ActionID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_RolesActions_Actions_ActionID",
                        column: x => x.ActionID,
                        principalSchema: "Security",
                        principalTable: "Actions",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_RolesActions_Roles_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RolesMenus",
                schema: "Security",
                columns: table => new
                {
                    MenuID = table.Column<long>(type: "bigint", nullable: false),
                    RoleID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesMenus", x => new { x.MenuID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_RolesMenus_Menus_MenuID",
                        column: x => x.MenuID,
                        principalSchema: "Security",
                        principalTable: "Menus",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_RolesMenus_Roles_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RolesUsers",
                schema: "Security",
                columns: table => new
                {
                    RoleID = table.Column<long>(type: "bigint", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesUsers", x => new { x.RoleID, x.UserID });
                    table.ForeignKey(
                        name: "FK_RolesUsers_Roles_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "Security",
                        principalTable: "Roles",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_RolesUsers_Users_UserID",
                        column: x => x.UserID,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "OperationsHistory",
                schema: "Audit",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityID = table.Column<long>(type: "bigint", nullable: true),
                    EntityName = table.Column<string>(type: "VARCHAR(100)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ServiceHistoryID = table.Column<long>(type: "bigint", nullable: false),
                    TableName = table.Column<string>(type: "VARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationsHistory", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OperationsHistory_ServicesHistory_ServiceHistoryID",
                        column: x => x.ServiceHistoryID,
                        principalSchema: "Audit",
                        principalTable: "ServicesHistory",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actions_CreatedByID",
                schema: "Security",
                table: "Actions",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_LastChangedByID",
                schema: "Security",
                table: "Actions",
                column: "LastChangedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_CreatedByID",
                schema: "Security",
                table: "Menus",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_LastChangedByID",
                schema: "Security",
                table: "Menus",
                column: "LastChangedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_ParentMenuID",
                schema: "Security",
                table: "Menus",
                column: "ParentMenuID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsHistory_ServiceHistoryID",
                schema: "Audit",
                table: "OperationsHistory",
                column: "ServiceHistoryID");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserID",
                schema: "Security",
                table: "RefreshTokens",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedByID",
                schema: "Security",
                table: "Roles",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_LastChangedByID",
                schema: "Security",
                table: "Roles",
                column: "LastChangedByID");

            migrationBuilder.CreateIndex(
                name: "IX_RolesActions_RoleID",
                schema: "Security",
                table: "RolesActions",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_RolesMenus_RoleID",
                schema: "Security",
                table: "RolesMenus",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_RolesUsers_UserID",
                schema: "Security",
                table: "RolesUsers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ServicesHistory_ChangedByID",
                schema: "Audit",
                table: "ServicesHistory",
                column: "ChangedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedByID",
                schema: "Security",
                table: "Users",
                column: "CreatedByID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastChangedByID",
                schema: "Security",
                table: "Users",
                column: "LastChangedByID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationsHistory",
                schema: "Audit");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "RolesActions",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "RolesMenus",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "RolesUsers",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "ServicesHistory",
                schema: "Audit");

            migrationBuilder.DropTable(
                name: "Actions",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Menus",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Security");
        }
    }
}
