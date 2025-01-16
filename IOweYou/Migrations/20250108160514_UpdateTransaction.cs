using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOweYou.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balance_Currency_CurrencyId",
                table: "Balance");

            migrationBuilder.DropForeignKey(
                name: "FK_Balance_Users_FromUserId",
                table: "Balance");

            migrationBuilder.DropForeignKey(
                name: "FK_Balance_Users_ToUserId",
                table: "Balance");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Currency_CurrencyID",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Balance",
                table: "Balance");

            migrationBuilder.RenameTable(
                name: "Currency",
                newName: "Currencies");

            migrationBuilder.RenameTable(
                name: "Balance",
                newName: "Balances");

            migrationBuilder.RenameIndex(
                name: "IX_Balance_ToUserId",
                table: "Balances",
                newName: "IX_Balances_ToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Balance_FromUserId",
                table: "Balances",
                newName: "IX_Balances_FromUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Balance_CurrencyId",
                table: "Balances",
                newName: "IX_Balances_CurrencyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP(6)",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Balances",
                table: "Balances",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_Currencies_CurrencyId",
                table: "Balances",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_Users_FromUserId",
                table: "Balances",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_Users_ToUserId",
                table: "Balances",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Currencies_CurrencyID",
                table: "Transactions",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_Currencies_CurrencyId",
                table: "Balances");

            migrationBuilder.DropForeignKey(
                name: "FK_Balances_Users_FromUserId",
                table: "Balances");

            migrationBuilder.DropForeignKey(
                name: "FK_Balances_Users_ToUserId",
                table: "Balances");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Currencies_CurrencyID",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Balances",
                table: "Balances");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "Currency");

            migrationBuilder.RenameTable(
                name: "Balances",
                newName: "Balance");

            migrationBuilder.RenameIndex(
                name: "IX_Balances_ToUserId",
                table: "Balance",
                newName: "IX_Balance_ToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Balances_FromUserId",
                table: "Balance",
                newName: "IX_Balance_FromUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Balances_CurrencyId",
                table: "Balance",
                newName: "IX_Balance_CurrencyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "CURRENT_TIMESTAMP(6)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Balance",
                table: "Balance",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Balance_Currency_CurrencyId",
                table: "Balance",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Balance_Users_FromUserId",
                table: "Balance",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Balance_Users_ToUserId",
                table: "Balance",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Currency_CurrencyID",
                table: "Transactions",
                column: "CurrencyID",
                principalTable: "Currency",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
