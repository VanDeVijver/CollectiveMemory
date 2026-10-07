using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectiveMemory.Core.Migrations
{
    /// <inheritdoc />
    public partial class SvenImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 6,
                column: "Image",
                value: "Sven.webp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 6,
                column: "Image",
                value: "placeholder");
        }
    }
}
