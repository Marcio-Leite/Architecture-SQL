using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerSQL.Migrations
{
    public partial class InitialIdentity3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserRoles_ApplicationRole_ApplicationRoleId",
                table: "ApplicationUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserRoles_ApplicationUser_ApplicationUserId",
                table: "ApplicationUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationRole",
                table: "ApplicationRole");

            migrationBuilder.RenameTable(
                name: "ApplicationUser",
                newName: "ApplicationUsers");

            migrationBuilder.RenameTable(
                name: "ApplicationRole",
                newName: "ApplicationRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUsers",
                table: "ApplicationUsers",
                column: "ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationRoles",
                table: "ApplicationRoles",
                column: "ApplicationRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserRoles_ApplicationRoles_ApplicationRoleId",
                table: "ApplicationUserRoles",
                column: "ApplicationRoleId",
                principalTable: "ApplicationRoles",
                principalColumn: "ApplicationRoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserRoles_ApplicationUsers_ApplicationUserId",
                table: "ApplicationUserRoles",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserRoles_ApplicationRoles_ApplicationRoleId",
                table: "ApplicationUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserRoles_ApplicationUsers_ApplicationUserId",
                table: "ApplicationUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUsers",
                table: "ApplicationUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationRoles",
                table: "ApplicationRoles");

            migrationBuilder.RenameTable(
                name: "ApplicationUsers",
                newName: "ApplicationUser");

            migrationBuilder.RenameTable(
                name: "ApplicationRoles",
                newName: "ApplicationRole");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUser",
                table: "ApplicationUser",
                column: "ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationRole",
                table: "ApplicationRole",
                column: "ApplicationRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserRoles_ApplicationRole_ApplicationRoleId",
                table: "ApplicationUserRoles",
                column: "ApplicationRoleId",
                principalTable: "ApplicationRole",
                principalColumn: "ApplicationRoleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserRoles_ApplicationUser_ApplicationUserId",
                table: "ApplicationUserRoles",
                column: "ApplicationUserId",
                principalTable: "ApplicationUser",
                principalColumn: "ApplicationUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
