using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOweYou.Migrations
{
    /// <inheritdoc />
    public partial class EditTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Received",
                table: "Transactions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Received",
                table: "Transactions");
        }
    }
}
