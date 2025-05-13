using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testApp.Migrations
{
    /// <inheritdoc />
    public partial class AddFarmerLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FarmerId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FarmerId",
                table: "Users",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Farmers_FarmerId",
                table: "Users",
                column: "FarmerId",
                principalTable: "Farmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Farmers_FarmerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FarmerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FarmerId",
                table: "Users");
        }
    }
}
