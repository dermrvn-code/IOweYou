using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOweYou.Migrations
{
    /// <inheritdoc />
    public partial class Adddbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserToken_Users_UserID",
                table: "UserToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserToken",
                table: "UserToken");

            migrationBuilder.RenameTable(
                name: "UserToken",
                newName: "UserTokens");

            migrationBuilder.RenameIndex(
                name: "IX_UserToken_UserID",
                table: "UserTokens",
                newName: "IX_UserTokens_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTokens",
                table: "UserTokens",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_Users_UserID",
                table: "UserTokens",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_Users_UserID",
                table: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTokens",
                table: "UserTokens");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                newName: "UserToken");

            migrationBuilder.RenameIndex(
                name: "IX_UserTokens_UserID",
                table: "UserToken",
                newName: "IX_UserToken_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserToken",
                table: "UserToken",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserToken_Users_UserID",
                table: "UserToken",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
