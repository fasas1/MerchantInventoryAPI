using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MechantInventory.Migrations
{
    /// <inheritdoc />
    public partial class AddItemsToResellerTrxiItemsAndOthers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_ResellerTransactions_TransactionId",
                table: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_AuditLogs_TransactionId",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ResellerTransactionItems");

            migrationBuilder.AddColumn<int>(
                name: "ReturnedQuantity",
                table: "ResellerTransactionItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ResellerReturnItem",
                columns: table => new
                {
                    ResellerReturnItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResellerReturnId = table.Column<int>(type: "int", nullable: false),
                    ResellerTransactionItemId = table.Column<int>(type: "int", nullable: false),
                    QuantityReturned = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResellerReturnItem", x => x.ResellerReturnItemId);
                    table.ForeignKey(
                        name: "FK_ResellerReturnItem_ResellerReturns_ResellerReturnId",
                        column: x => x.ResellerReturnId,
                        principalTable: "ResellerReturns",
                        principalColumn: "ResellerReturnId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResellerReturnItem_ResellerTransactionItems_ResellerTransactionItemId",
                        column: x => x.ResellerTransactionItemId,
                        principalTable: "ResellerTransactionItems",
                        principalColumn: "ResellerTransactionItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResellerReturnItem_ResellerReturnId",
                table: "ResellerReturnItem",
                column: "ResellerReturnId");

            migrationBuilder.CreateIndex(
                name: "IX_ResellerReturnItem_ResellerTransactionItemId",
                table: "ResellerReturnItem",
                column: "ResellerTransactionItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResellerReturnItem");

            migrationBuilder.DropColumn(
                name: "ReturnedQuantity",
                table: "ResellerTransactionItems");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "ResellerTransactionItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TransactionId",
                table: "AuditLogs",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_ResellerTransactions_TransactionId",
                table: "AuditLogs",
                column: "TransactionId",
                principalTable: "ResellerTransactions",
                principalColumn: "ResellerTransactionId");
        }
    }
}
