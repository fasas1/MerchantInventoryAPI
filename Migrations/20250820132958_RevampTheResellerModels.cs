using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MechantInventory.Migrations
{
    /// <inheritdoc />
    public partial class RevampTheResellerModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResellerReturnItem");

            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "ResellerTransactions");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "ResellerTransactions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ResellerTransactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ResellerTransactions");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "ResellerTransactionItems");

            migrationBuilder.DropColumn(
                name: "CreditBalance",
                table: "Resellers");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "ResellerReturns");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "ResellerReturns");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "ResellerReturns");

            migrationBuilder.DropColumn(
                name: "ActualAmountPaid",
                table: "ResellerPayments");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "ResellerPayments");

            migrationBuilder.RenameColumn(
                name: "OverPayment",
                table: "ResellerTransactions",
                newName: "PaidAmount");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ResellerTransactions",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ResellerTransactionItems",
                newName: "ResellerTransactionItemId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "ResellerTransactions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ResellerReturns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ResellerReturns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ResellerReturns_ProductId",
                table: "ResellerReturns",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResellerReturns_Products_ProductId",
                table: "ResellerReturns",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResellerReturns_Products_ProductId",
                table: "ResellerReturns");

            migrationBuilder.DropIndex(
                name: "IX_ResellerReturns_ProductId",
                table: "ResellerReturns");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ResellerReturns");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ResellerReturns");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "ResellerTransactions",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "PaidAmount",
                table: "ResellerTransactions",
                newName: "OverPayment");

            migrationBuilder.RenameColumn(
                name: "ResellerTransactionItemId",
                table: "ResellerTransactionItems",
                newName: "Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "ResellerTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountPaid",
                table: "ResellerTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "ResellerTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ResellerTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ResellerTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "ResellerTransactionItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CreditBalance",
                table: "Resellers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "ResellerReturns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "ResellerReturns",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "ResellerReturns",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ActualAmountPaid",
                table: "ResellerPayments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "ResellerPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ResellerReturnItem",
                columns: table => new
                {
                    ResellerReturnItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ResellerReturnId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResellerReturnItem", x => x.ResellerReturnItemId);
                    table.ForeignKey(
                        name: "FK_ResellerReturnItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ResellerReturnItem_ResellerReturns_ResellerReturnId",
                        column: x => x.ResellerReturnId,
                        principalTable: "ResellerReturns",
                        principalColumn: "ResellerReturnId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResellerReturnItem_ProductId",
                table: "ResellerReturnItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ResellerReturnItem_ResellerReturnId",
                table: "ResellerReturnItem",
                column: "ResellerReturnId");
        }
    }
}
