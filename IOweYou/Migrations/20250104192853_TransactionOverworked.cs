using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOweYou.Migrations
{
    /// <inheritdoc />
    public partial class TransactionOverworked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "Users",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP(6)",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyID",
                table: "Transactions",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PartnerID",
                table: "Transactions",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    UnitValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Balance",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    FromUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ToUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Balance_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Balance_Users_FromUserId",
                        column: x => x.FromUserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Balance_Users_ToUserId",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CurrencyID",
                table: "Transactions",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PartnerID",
                table: "Transactions",
                column: "PartnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Balance_CurrencyId",
                table: "Balance",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Balance_FromUserId",
                table: "Balance",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Balance_ToUserId",
                table: "Balance",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Currency_CurrencyID",
                table: "Transactions",
                column: "CurrencyID",
                principalTable: "Currency",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Users_PartnerID",
                table: "Transactions",
                column: "PartnerID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Currency_CurrencyID",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Users_PartnerID",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Balance");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CurrencyID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PartnerID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrencyID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PartnerID",
                table: "Transactions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "CURRENT_TIMESTAMP(6)");
        }
    }
}
