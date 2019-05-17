using Microsoft.EntityFrameworkCore.Migrations;

namespace Njoy.Data.Migrations
{
    public partial class AddressPartsConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Districts_CountyId",
                table: "Districts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Districts",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Counties",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Districts_CountyId_Name",
                table: "Districts",
                columns: new[] { "CountyId", "Name" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Counties_Name_CityId",
                table: "Counties",
                columns: new[] { "Name", "CityId" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Cities_LicensePlateCode",
                table: "Cities",
                column: "LicensePlateCode");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Cities_Name",
                table: "Cities",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Districts_CountyId_Name",
                table: "Districts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Counties_Name_CityId",
                table: "Counties");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Cities_LicensePlateCode",
                table: "Cities");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Cities_Name",
                table: "Cities");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Districts",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Counties",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CountyId",
                table: "Districts",
                column: "CountyId");
        }
    }
}
