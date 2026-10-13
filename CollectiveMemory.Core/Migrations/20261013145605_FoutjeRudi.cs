using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectiveMemory.Core.Migrations
{
    /// <inheritdoc />
    public partial class FoutjeRudi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Bands", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "DOZ-band", "Collective Memory XL" }, new List<string> { "Brede Mix van stijlen" }, new List<string> { "Piano" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Bands", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "DOZ-band", "Collective Memory XL" }, new List<string> { "Meerstemmige Rock", "Meerstemmige Pop", "'70s" }, new List<string> { "Gitaar", "Vocals" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Bands", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "The Monkey Buttlets Bigband", "Collective Memory XL", "The Augmented Combo", "DOZ-band" }, new List<string> { "Jazz", "Pop", "Bluesrock" }, new List<string> { "Trommel", "Drum" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "Collective Memory XL" }, "Rudi is een man met al tal van jaren ervaring binnen de muziekwereld, neem maar eens een kijkje naar zijn repertoire: jaren 80: UNION met oa. Peter Vermeersch; jaren 2006-2025: Collective Memory; jaren 2010-2026: The Noise Dikkelvenne; jaren 2017-2018: Piloot met Bert Villa; en nu dus ook kan hij zijn expertise en liefde voor muziek delen in Collective Memory XL", new List<string> { "TO BE INSERTED" }, new List<string> { "Gitaar", "Vocals" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Bands", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "Collective Memory XL", "Black on light", "DOZ-band" }, new List<string> { "Dave Matthews Band", "The Doors" }, new List<string> { "Bass", "Zang" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Bands", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "Collective Memory XL" }, new List<string> { "TO BE INSERTED" }, new List<string> { "Gitaar", "Vocals" } });

            migrationBuilder.UpdateData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: 1,
                column: "AdditionalLinks",
                value: new List<string> { "https://www.facebook.com/permalink.php?story_fbid=pfbid02H5csbRFDDz5zoN9pLhJx4cUTwU7CLDePvozkTyqa7DUGDYYxRdTzN2akM1iAfrL2l&id=61566590114083" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Bands", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "DOZ-band", "Collective Memory XL" }, new List<string> { "Brede Mix van stijlen" }, new List<string> { "Piano" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Bands", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "DOZ-band", "Collective Memory XL" }, new List<string> { "Meerstemmige Rock", "Meerstemmige Pop", "'70s" }, new List<string> { "Gitaar", "Vocals" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Bands", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "The Monkey Buttlets Bigband", "Collective Memory XL", "The Augmented Combo", "DOZ-band" }, new List<string> { "Jazz", "Pop", "Bluesrock" }, new List<string> { "Trommel", "Drum" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "Collective Memory XL" }, "Rudi is een man met al tal van jaren ervaring binnen de muziekwereld, neem maar eens een kijkje naar zijn repertoire: [13:51, 13-10-2026] Seppe Van De Vijver: Top merci\r\n[13:51, 13-10-2026] Bert: jaren 80: UNION met oa. Peter Vermeersch; jaren 2006-2025: Collective Memory; jaren 2010-2026: The Noise Dikkelvenne; jaren 2017-2018: Piloot met Bert Villa; en nu dus ook kan hij zijn expertise en liefde voor muziek delen in Collective Memory XL", new List<string> { "TO BE INSERTED" }, new List<string> { "Gitaar", "Vocals" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Bands", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "Collective Memory XL", "Black on light", "DOZ-band" }, new List<string> { "Dave Matthews Band", "The Doors" }, new List<string> { "Bass", "Zang" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Bands", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "Collective Memory XL" }, new List<string> { "TO BE INSERTED" }, new List<string> { "Gitaar", "Vocals" } });

            migrationBuilder.UpdateData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: 1,
                column: "AdditionalLinks",
                value: new List<string> { "https://www.facebook.com/permalink.php?story_fbid=pfbid02H5csbRFDDz5zoN9pLhJx4cUTwU7CLDePvozkTyqa7DUGDYYxRdTzN2akM1iAfrL2l&id=61566590114083" });
        }
    }
}
