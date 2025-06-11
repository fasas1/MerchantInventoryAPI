using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MechantInventory.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Category", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Watch", "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691699385/watch-prod-3_mvvw3x.webp", "Iwatch Series 5", 180000.00m },
                    { 2, "Phone", "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691699516/iphone_14_purple_ck04dl.png", "Iphone 15 Promax", 1400000.00m },
                    { 3, "Phone", "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691669759/iphone12_fid8e0.png", "Iphone 14 Pro", 900000.00m },
                    { 4, "Headset", "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691699439/headphone-prod-2_bmgfov.webp", "Boom Headset", 130000.00m },
                    { 5, "Airpod", "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691871784/earbuds-prod-4_la7u7j.webp", "Oraimo Airpod", 34000.00m },
                    { 6, "Phone", "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691871777/13pro_blue_ws4pjl.png", "Iphone 13 Pro", 650000.00m },
                    { 7, "Watch", "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691871415/watch-prod-2_eya0ym.webp", "Samsung Watch-7", 280000.00m },
                    { 8, "Headset", "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691699650/headphone-prod-5_ayglse.webp", "Zolum Headset", 125000.00m },
                    { 10, "Macbook", "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1685574593/Mabook_Pro_aicpdz.png", "Macbook Pro 2022", 2155000.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10);
        }
    }
}
