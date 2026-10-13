using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectiveMemory.Core.Migrations
{
    /// <inheritdoc />
    public partial class MemberBios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "DOZ-band", "Collective Memory XL" }, "Piano spelen is al sinds haar kindertijd een rode draad in haar leven.In 2024 zette ze haar eerste stappen in een band bij de DOZ band. Via DOZ kwam ze vervolgens bij Collective Memory Xl terecht. Muziek betekent voor haar zoveel meer dan alleen noten spelen: het raakt, brengt herinneringen tot leven en geeft de ruimte om emoties te voelen en te delen. Bij Collective Memory XL kan ze haar liefde en gevoel voor muziek delen en samen met de band het publiek meenemen op een muzikale reis vol herkenning,warmte en emotie.", new List<string> { "Brede Mix van stijlen" }, new List<string> { "TO BE INSERTED" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "DOZ-band", "Collective Memory XL" }, "Eric is al dertig jaar de vaste stem en gitarist van DOZband, een trouwe waarde die het geluid van de groep mee vorm heeft gegeven. Daarnaast stond hij als zanger-gitarst ook bij Collective Memory, en vandaag zet hij dat verhaal verder bij Collective Memory XL. Wat hem echt drijft zijn meerstemmige harmonieën: het moment waarop stemmen in elkaar vallen en samen meer worden dan de som van de delen. Die passie hoor je in elk nummer dat hij mee brengt.", new List<string> { "Meerstemmige Rock", "Meerstemmige Pop", "'70s" }, new List<string> { "Gitaar", "Vocals" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "The Monkey Buttlets Bigband", "Collective Memory XL", "The Augmented Combo", "DOZ-band" }, "Begonnen als trommelaar in de fanfare en doorgegroeid als drummer aan de muziekschool van Oudenaarde. Na jaren actief te zijn in fanfares en harmonieën, volledig zijn ding gevonden in de bigbandmuziek en nu een nieuwe uitdaging gevonden bij Collective Memory XL.", new List<string> { "Jazz", "Pop", "Bluesrock" }, new List<string> { "Trommel", "Drum" } });

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
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "Collective Memory XL", "Black on light", "DOZ-band" }, "Bert zijn hele leven staat in teken van muziek. In zijn jonge jaren leerde hij al de fundamenten van muziek aan het conservatorium vervolgens scherpte hij zijn kennis omtrent producing bij. Hierna bracht hij 'like-minded' mensen samen in muziekcafé de giraf en in het heden spendeert hij zijn vrije tijd op zijn bassgitaar onder de vlag van onder andere DOZband, Black On Light en nu dus ook Collective Memory Xl", new List<string> { "Dave Matthews Band", "The Doors" }, new List<string> { "Bass", "Zang" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "Collective Memory XL" }, "Sven kiest bij Collective Memory XL resoluut voor een old-school warme analoge sound als fundament en ondersteunt mee de vocale harmonïeën. Zijn liefde voor pure klank, handgemaakte gear en zijn ervaring met historische orgelklanken als organist en orgelbouwer, aangevuld met diverse gitaarprojecten van partybands tot eigen muziek, maken hem een muzikant die voordurend met kleur en textuur werkt.", new List<string> { "TO BE INSERTED" }, new List<string> { "Gitaar", "Vocals" } });

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
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "DOZ-band", "Collective Memory XL" }, "Met een brede muzikale smaak en veel goesting om samen te spelen, is Katrientje Verhaert actief bij DOZ-band en Collective Memory XL. Gedreven door samenspel en muzikale nieuwsgierigheid zoekt ze steeds naar connectie in muziek.", new List<string> { "Brede Mix van stijlen" }, new List<string> { "TO BE INSERTED" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "DOZ-band", "Collective Memory XL" }, "TO BE INSERTED", new List<string> { "Meerstemmige Rock", "Meerstemmige Pop", "'70s" }, new List<string> { "Gitaar", "Vocals" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "The Monkey Buttlets Bigband", "Collective Memory XL", "The Augmented Combo", "DOZ-band" }, "Begonnen als trommelaar in de fanfare en doorgegroeid als drummer aan de muziekschool van Oudenaarde. Na jaren actief te zijn in fanfares en harmonieën, volledig zijn ding gevonden in de bigbandmuziek en nu een nieuwe uitdaging gevonden bij Collective Memory.", new List<string> { "Jazz", "Pop", "Bluesrock" }, new List<string> { "Trommel", "Drum" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "Collective Memory XL" }, "TO BE INSERTED", new List<string> { "TO BE INSERTED" }, new List<string> { "Gitaar", "Vocals" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "Collective Memory XL", "Black on light", "DOZ-band" }, "TO BE INSERTED", new List<string> { "Dave Matthews Band", "The Doors" }, new List<string> { "Bass", "Zang" } });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Bands", "Bio", "FavoriteMusic", "Instruments" },
                values: new object[] { new List<string> { "Collective Memory XL" }, "TO BE INSERTED", new List<string> { "TO BE INSERTED" }, new List<string> { "Gitaar", "Vocals" } });

            migrationBuilder.UpdateData(
                table: "Shows",
                keyColumn: "Id",
                keyValue: 1,
                column: "AdditionalLinks",
                value: new List<string> { "https://www.facebook.com/permalink.php?story_fbid=pfbid02H5csbRFDDz5zoN9pLhJx4cUTwU7CLDePvozkTyqa7DUGDYYxRdTzN2akM1iAfrL2l&id=61566590114083" });
        }
    }
}
