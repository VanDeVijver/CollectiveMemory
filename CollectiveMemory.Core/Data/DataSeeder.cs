using CollectiveMemory.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CollectiveMemory.Core.Data
{
    public static class DataSeeder
    {
        private const string AdminRoleId = "00000000-0000-0000-0000-000000000001";
        private const string AdminRoleName = "Admin";

        // Static seed date — never changes, never causes spurious migrations
        private static readonly DateTime SeedDate =
            new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static void Seed(ModelBuilder builder)
        {
            SeedRoles(builder);
            SeedMembers(builder);
            SeedShows(builder);
            SeedClips(builder);
        }

        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = AdminRoleId,
                    Name = AdminRoleName,
                    NormalizedName = AdminRoleName.ToUpper(),
                    ConcurrencyStamp = "static-role-stamp-001" // static — no spurious migrations
                }
            );
        }

        private static void SeedMembers(ModelBuilder builder)
        {
            builder.Entity<Member>().HasData(
                new Member
                {
                    Id = 1,
                    Firstname = "Katrientje",
                    Lastname = "Verhaert",
                    Bands = new List<string> { "DOZ-band", "Collective Memory XL" },
                    FavoriteMusic = new List<string> { "Brede Mix van stijlen" },
                    Instruments = new List<string> { "Piano" },
                    Bio = "Piano spelen is al sinds haar kindertijd een rode draad in haar leven." +
                    "In 2024 zette ze haar eerste stappen in een band bij de DOZ band. Via DOZ kwam ze vervolgens bij Collective Memory" +
                    " Xl terecht. Muziek betekent voor haar zoveel meer dan alleen noten spelen: het raakt," +
                    " brengt herinneringen tot leven en geeft de ruimte om emoties te voelen en te delen. Bij Collective Memory XL " +
                    "kan ze haar liefde en gevoel voor muziek delen en samen met de band het publiek meenemen op een " +
                    "muzikale reis vol herkenning,warmte en emotie.",
                    Image = "Katrientje.webp",
                    CreatedAt = SeedDate
                },
                new Member
                {
                    Id = 2,
                    Firstname = "Eric",
                    Lastname = "De Cuyper",
                    Bands = new List<string> { "DOZ-band", "Collective Memory XL" },
                    FavoriteMusic = new List<string> { "Meerstemmige Rock", "Meerstemmige Pop", "'70s" },
                    Instruments = new List<string> { "Gitaar", "Vocals" },
                    Bio = "Eric is al dertig jaar de vaste stem en gitarist van DOZband, een trouwe waarde die het geluid " +
                    "van de groep mee vorm heeft gegeven. Daarnaast stond hij als zanger-gitarst ook bij Collective Memory, en vandaag" +
                    " zet hij dat verhaal verder bij Collective Memory XL. Wat hem echt drijft zijn meerstemmige harmonieën: het moment " +
                    "waarop stemmen in elkaar vallen en samen meer worden dan de som van de delen. Die passie hoor je in elk nummer dat hij mee brengt.",
                    Image = "Eric.webp",
                    CreatedAt = SeedDate
                },
                new Member
                {
                    Id = 3,
                    Firstname = "Lorenz",
                    Lastname = "Duytschaever",
                    Bands = new List<string> { "The Monkey Buttlets Bigband", "Collective Memory XL",
                                                       "The Augmented Combo", "DOZ-band" },
                    FavoriteMusic = new List<string> { "Jazz", "Pop", "Bluesrock" },
                    Instruments = new List<string> { "Trommel", "Drum" },
                    Bio = "Begonnen als trommelaar in de fanfare en doorgegroeid als drummer aan de muziekschool van Oudenaarde." +
                                    " Na jaren actief te zijn in fanfares en harmonieën, volledig zijn ding gevonden in de bigbandmuziek" +
                                    " en nu een nieuwe uitdaging gevonden bij Collective Memory XL.",
                    Image = "Lorenz.webp",
                    CreatedAt = SeedDate
                },
                new Member
                {
                    Id = 4,
                    Firstname = "Rudi",
                    Lastname = "De Backer",
                    Bands = new List<string> { "Collective Memory XL" },
                    FavoriteMusic = new List<string> { "TO BE INSERTED" },
                    Instruments = new List<string> { "Gitaar", "Vocals" },
                    Bio = "Rudi is een man met al tal van jaren ervaring binnen de muziekwereld, neem maar eens een kijkje naar zijn repertoire:" +
                    " jaren 80: UNION met oa. Peter Vermeersch; jaren 2006-2025:" +
                    " Collective Memory; jaren 2010-2026: The Noise Dikkelvenne; jaren 2017-2018: Piloot met Bert Villa;" +
                    " en dus nu kan hij ook zijn expertise en liefde voor muziek delen in Collective Memory XL",
                    Image = "Rudi.webp",
                    CreatedAt = SeedDate
                },
                new Member
                {
                    Id = 5,
                    Firstname = "Bert",
                    Lastname = "Denayer",
                    Bands = new List<string> { "Collective Memory XL", "Black on light", "DOZ-band" },
                    FavoriteMusic = new List<string> { "Dave Matthews Band", "The Doors" },
                    Instruments = new List<string> { "Bass", "Zang" },
                    Bio = "Bert zijn hele leven staat in teken van muziek. In zijn jonge jaren leerde hij al de fundamenten van muziek in de \"Rockfabriek \" en aan het conservatorium. " +
                    "Hierna bracht hij like-minded mensen samen in muziekcafé \"De Giraf\" en " +
                    "in het heden spendeert hij zijn vrije tijd op zijn bassgitaar onder de vlag van onder andere DOZband, Black On Light en nu dus ook Collective Memory Xl",
                    Image = "Bert.webp",
                    CreatedAt = SeedDate
                },
                new Member
                {
                    Id = 6,
                    Firstname = "Sven",
                    Lastname = "Vermassen",
                    Bands = new List<string> { "Collective Memory XL" },
                    FavoriteMusic = new List<string> { "TO BE INSERTED" },
                    Instruments = new List<string> { "Gitaar", "Vocals" },
                    Bio = "Sven kiest bij Collective Memory XL resoluut voor een old-school warme analoge sound " +
                    "als fundament en ondersteunt mee de vocale harmonïeën. Zijn liefde voor pure klank, handgemaakte " +
                    "gear en zijn ervaring met historische orgelklanken als organist en orgelbouwer, aangevuld met diverse " +
                    "gitaarprojecten van partybands tot eigen muziek, maken hem een muzikant die voordurend met kleur en textuur werkt.",
                    Image = "Sven.webp",
                    CreatedAt = SeedDate
                }
            );
        }

        private static void SeedShows(ModelBuilder builder)
        {
            builder.Entity<Show>().HasData(
                new Show
                {
                    Id = 1,
                    Venue = "Isorex Arena",
                    City = "Gavere",
                    Date = new DateTime(2026, 4, 30, 21, 0, 0, DateTimeKind.Utc), // ← Utc kind
                    AdditionalInfo = "Dit evenement wordt georganiseerd voor het goede doel," +
                                     " meer info kan u vinden op deze link:",
                    AdditionalLinks = new List<string>
                    {
                        "https://www.facebook.com/permalink.php?story_fbid=pfbid02H5csbRFDDz5zoN9pLhJx4cUTwU7CLDePvozkTyqa7DUGDYYxRdTzN2akM1iAfrL2l&id=61566590114083"
                    },
                    Price = 35.00,
                    CreatedAt = SeedDate
                },
                new Show
                {
                    Id = 2,
                    Venue = "De Kiem",
                    City = "Gavere",
                    Street = "Vluchtenboerstraat",
                    StreetNumber = "7a",
                    Date = new DateTime(2026, 5, 30, 19, 0, 0, DateTimeKind.Utc), // ← Utc kind
                    AdditionalInfo = "Voor de 50ste verjaardag van De Kiem organiseren we een show" +
                                     " die zich inzet voor het goede doel.",
                    Price = 0.0,
                    CreatedAt = SeedDate
                }
            );
        }

        private static void SeedClips(ModelBuilder builder)
        {
            // The three clips cut from the Isorex Arena try-out, served from wwwroot/media/clips.
            builder.Entity<Clip>().HasData(
                Enumerable.Range(1, 3).Select(n => new Clip
                {
                    Id = n,
                    Title = "Live at Isorex Arena",
                    Subtitle = "Gavere · Apr 2026",
                    VideoUrl = $"/media/clips/clip-{n}.mp4",
                    PosterUrl = $"/media/clips/posters/clip-{n}.jpg",
                    CreatedAt = SeedDate
                }).ToArray()
            );
        }
    }
}
