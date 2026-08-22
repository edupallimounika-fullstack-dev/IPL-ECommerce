using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IPL.ECommerce.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Franchises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Franchises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FranchiseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Franchises_FranchiseId",
                        column: x => x.FranchiseId,
                        principalTable: "Franchises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Franchises",
                columns: new[] { "Id", "Code", "CreatedDate", "IsActive", "LogoUrl", "Name" },
                values: new object[,]
                {
                    { 1, "CSK", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "/images/franchises/csk.png", "Chennai Super Kings" },
                    { 2, "MI", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "/images/franchises/mi.png", "Mumbai Indians" },
                    { 3, "RCB", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "/images/franchises/rcb.png", "Royal Challengers Bengaluru" },
                    { 4, "KKR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "/images/franchises/kkr.png", "Kolkata Knight Riders" },
                    { 5, "SRH", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "/images/franchises/srh.png", "Sunrisers Hyderabad" },
                    { 6, "RR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "/images/franchises/rr.png", "Rajasthan Royals" },
                    { 7, "DC", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "/images/franchises/dc.png", "Delhi Capitals" },
                    { 8, "PBKS", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "/images/franchises/pbks.png", "Punjab Kings" },
                    { 9, "GT", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "/images/franchises/gt.png", "Gujarat Titans" },
                    { 10, "LSG", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "/images/franchises/lsg.png", "Lucknow Super Giants" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedDate", "Description", "FranchiseId", "ImageUrl", "IsActive", "ModifiedDate", "Name", "Price", "ProductType", "StockQuantity" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Official Chennai Super Kings jersey.", 1, "/images/products/csk-jersey.png", true, null, "CSK Official Jersey", 2999.00m, 1, 100 },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Official Chennai Super Kings cap.", 1, "/images/products/csk-cap.png", true, null, "CSK Official Cap", 999.00m, 2, 150 },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chennai Super Kings supporter flag.", 1, "/images/products/csk-flag.png", true, null, "CSK Supporter Flag", 499.00m, 3, 200 },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Official Mumbai Indians jersey.", 2, "/images/products/mi-jersey.png", true, null, "MI Official Jersey", 2999.00m, 1, 100 },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Official Mumbai Indians cap.", 2, "/images/products/mi-cap.png", true, null, "MI Official Cap", 999.00m, 2, 150 },
                    { 6, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Official Royal Challengers Bengaluru jersey.", 3, "/images/products/rcb-jersey.png", true, null, "RCB Official Jersey", 2999.00m, 1, 100 },
                    { 7, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Official Royal Challengers Bengaluru cap.", 3, "/images/products/rcb-cap.png", true, null, "RCB Official Cap", 999.00m, 2, 150 },
                    { 8, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Official Kolkata Knight Riders jersey.", 4, "/images/products/kkr-jersey.png", true, null, "KKR Official Jersey", 2999.00m, 1, 100 },
                    { 9, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Official Sunrisers Hyderabad jersey.", 5, "/images/products/srh-jersey.png", true, null, "SRH Official Jersey", 2999.00m, 1, 100 },
                    { 10, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Signed Chennai Super Kings photo.", 1, "/images/products/csk-signed-photo.png", true, null, "Autographed CSK Photo", 4999.00m, 4, 25 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Franchises_Code",
                table: "Franchises",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Franchises_Name",
                table: "Franchises",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_FranchiseId",
                table: "Products",
                column: "FranchiseId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_IsActive",
                table: "Products",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductType",
                table: "Products",
                column: "ProductType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Franchises");
        }
    }
}
