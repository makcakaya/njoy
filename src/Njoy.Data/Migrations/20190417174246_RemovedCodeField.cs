using Microsoft.EntityFrameworkCore.Migrations;

namespace Njoy.Data.Migrations
{
    public partial class RemovedCodeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Merchants_Code",
                table: "Merchants");

            migrationBuilder.DropIndex(
                name: "IX_Businesses_Code",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Businesses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Merchants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Businesses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Merchants_Code",
                table: "Merchants",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_Code",
                table: "Businesses",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");
        }
    }
}
