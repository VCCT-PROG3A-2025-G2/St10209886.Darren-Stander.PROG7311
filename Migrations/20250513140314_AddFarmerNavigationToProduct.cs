using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFarmerNavigationToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Products_FarmerId",
                table: "Products",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Farmers_FarmerId",
                table: "Products",
                column: "FarmerId",
                principalTable: "Farmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Farmers_FarmerId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FarmerId",
                table: "Products");
        }
    }
}
