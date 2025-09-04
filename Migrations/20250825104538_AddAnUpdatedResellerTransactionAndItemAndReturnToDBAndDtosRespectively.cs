using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MechantInventory.Migrations
{
    /// <inheritdoc />
    public partial class AddAnUpdatedResellerTransactionAndItemAndReturnToDBAndDtosRespectively : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                newName: "DueDate");

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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ResellerTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ResellerTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "OverPayment",
                table: "ResellerTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ResellerTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "ResellerTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "ResellerPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "ResellerTransactions");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "ResellerTransactions");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ResellerTransactions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ResellerTransactions");

            migrationBuilder.DropColumn(
                name: "OverPayment",
                table: "ResellerTransactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ResellerTransactions");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
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
                name: "PaymentMethod",
                table: "ResellerPayments");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "ResellerTransactions",
                newName: "TransactionDate");

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
    }
}
