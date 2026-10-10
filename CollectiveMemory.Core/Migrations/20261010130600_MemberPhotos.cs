using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectiveMemory.Core.Migrations
{
    /// <inheritdoc />
    public partial class MemberPhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                column: "Image",
                value: "Katrientje.webp");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                column: "Image",
                value: "Eric.webp");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                column: "Image",
                value: "Lorenz.webp");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 4,
                column: "Image",
                value: "Rudi.webp");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Firstname", "Image" },
                values: new object[] { "Bert", "Bert.webp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                column: "Image",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                column: "Image",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                column: "Image",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 4,
                column: "Image",
                value: "placeholder");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Firstname", "Image" },
                values: new object[] { "Bet", "placeholder" });
        }
    }
}
