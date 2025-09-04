using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MechantInventory.Migrations
{
    /// <inheritdoc />
    public partial class AddBalanceToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "ResellerTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "ResellerTransactions");
        }
    }
}
