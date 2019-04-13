using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Njoy.Data.Migrations
{
    public partial class IdentityRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminRoleClaims_AdminRoles_RoleId",
                table: "AdminRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AdminUserClaims_AdminUsers_UserId",
                table: "AdminUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AdminUserLogins_AdminUsers_UserId",
                table: "AdminUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AdminUserRoles_AdminRoles_RoleId",
                table: "AdminUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AdminUserRoles_AdminUsers_UserId",
                table: "AdminUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AdminUserTokens_AdminUsers_UserId",
                table: "AdminUserTokens");

            migrationBuilder.DropTable(
                name: "AdminRoles");

            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminUserTokens",
                table: "AdminUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminUserRoles",
                table: "AdminUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminUserLogins",
                table: "AdminUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminUserClaims",
                table: "AdminUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdminRoleClaims",
                table: "AdminRoleClaims");

            migrationBuilder.RenameTable(
                name: "AdminUserTokens",
                newName: "AppUserTokens");

            migrationBuilder.RenameTable(
                name: "AdminUserRoles",
                newName: "AppUserRoles");

            migrationBuilder.RenameTable(
                name: "AdminUserLogins",
                newName: "AppUserLogins");

            migrationBuilder.RenameTable(
                name: "AdminUserClaims",
                newName: "AppUserClaims");

            migrationBuilder.RenameTable(
                name: "AdminRoleClaims",
                newName: "AppRoleClaims");

            migrationBuilder.RenameIndex(
                name: "IX_AdminUserRoles_RoleId",
                table: "AppUserRoles",
                newName: "IX_AppUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AdminUserLogins_UserId",
                table: "AppUserLogins",
                newName: "IX_AppUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AdminUserClaims_UserId",
                table: "AppUserClaims",
                newName: "IX_AppUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AdminRoleClaims_RoleId",
                table: "AppRoleClaims",
                newName: "IX_AppRoleClaims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserTokens",
                table: "AppUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserRoles",
                table: "AppUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserLogins",
                table: "AppUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserClaims",
                table: "AppUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppRoleClaims",
                table: "AppRoleClaims",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AppRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AppUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AppUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRoleClaims_AppRoles_RoleId",
                table: "AppRoleClaims",
                column: "RoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserClaims_AppUsers_UserId",
                table: "AppUserClaims",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserLogins_AppUsers_UserId",
                table: "AppUserLogins",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoles_AppRoles_RoleId",
                table: "AppUserRoles",
                column: "RoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoles_AppUsers_UserId",
                table: "AppUserRoles",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserTokens_AppUsers_UserId",
                table: "AppUserTokens",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRoleClaims_AppRoles_RoleId",
                table: "AppRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserClaims_AppUsers_UserId",
                table: "AppUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserLogins_AppUsers_UserId",
                table: "AppUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoles_AppRoles_RoleId",
                table: "AppUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoles_AppUsers_UserId",
                table: "AppUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserTokens_AppUsers_UserId",
                table: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserTokens",
                table: "AppUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserRoles",
                table: "AppUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserLogins",
                table: "AppUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserClaims",
                table: "AppUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppRoleClaims",
                table: "AppRoleClaims");

            migrationBuilder.RenameTable(
                name: "AppUserTokens",
                newName: "AdminUserTokens");

            migrationBuilder.RenameTable(
                name: "AppUserRoles",
                newName: "AdminUserRoles");

            migrationBuilder.RenameTable(
                name: "AppUserLogins",
                newName: "AdminUserLogins");

            migrationBuilder.RenameTable(
                name: "AppUserClaims",
                newName: "AdminUserClaims");

            migrationBuilder.RenameTable(
                name: "AppRoleClaims",
                newName: "AdminRoleClaims");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserRoles_RoleId",
                table: "AdminUserRoles",
                newName: "IX_AdminUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserLogins_UserId",
                table: "AdminUserLogins",
                newName: "IX_AdminUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserClaims_UserId",
                table: "AdminUserClaims",
                newName: "IX_AdminUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppRoleClaims_RoleId",
                table: "AdminRoleClaims",
                newName: "IX_AdminRoleClaims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminUserTokens",
                table: "AdminUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminUserRoles",
                table: "AdminUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminUserLogins",
                table: "AdminUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminUserClaims",
                table: "AdminUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdminRoleClaims",
                table: "AdminRoleClaims",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AdminRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AdminRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AdminUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AdminUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminRoleClaims_AdminRoles_RoleId",
                table: "AdminRoleClaims",
                column: "RoleId",
                principalTable: "AdminRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminUserClaims_AdminUsers_UserId",
                table: "AdminUserClaims",
                column: "UserId",
                principalTable: "AdminUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminUserLogins_AdminUsers_UserId",
                table: "AdminUserLogins",
                column: "UserId",
                principalTable: "AdminUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminUserRoles_AdminRoles_RoleId",
                table: "AdminUserRoles",
                column: "RoleId",
                principalTable: "AdminRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminUserRoles_AdminUsers_UserId",
                table: "AdminUserRoles",
                column: "UserId",
                principalTable: "AdminUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminUserTokens_AdminUsers_UserId",
                table: "AdminUserTokens",
                column: "UserId",
                principalTable: "AdminUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
