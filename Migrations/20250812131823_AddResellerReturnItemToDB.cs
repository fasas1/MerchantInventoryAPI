using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MechantInventory.Migrations
{
    /// <inheritdoc />
    public partial class AddResellerReturnItemToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResellerReturnItem",
                columns: table => new
                {
                    ResellerReturnItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResellerReturnId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResellerReturnItem");
        }
    }
}
