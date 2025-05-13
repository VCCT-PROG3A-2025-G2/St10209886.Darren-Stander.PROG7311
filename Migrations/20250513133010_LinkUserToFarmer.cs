using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testApp.Migrations
{
    /// <inheritdoc />
    public partial class LinkUserToFarmer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_FarmerId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Farmers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FarmerId",
                table: "Users",
                column: "FarmerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_UserId",
                table: "Farmers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_Users_UserId",
                table: "Farmers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_Users_UserId",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Users_FarmerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_UserId",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Farmers");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FarmerId",
                table: "Users",
                column: "FarmerId");
        }
    }
}
