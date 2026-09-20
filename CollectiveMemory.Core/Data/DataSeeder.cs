using CollectiveMemory.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CollectiveMemory.Core.Data
{
    public static class DataSeeder
    {
        private const string AdminRoleId = "00000000-0000-0000-0000-000000000001";
        private const string AdminRoleName = "Admin";
        private const string AdminUserId = "00000000-0000-0000-0000-000000000001";
        private const string AdminFirstName = "Bert";
        private const string AdminLastName = "Denayer";
        private const string AdminEmail = "test@gmail.com";

        // Static seed date — never changes, never causes spurious migrations
        private static readonly DateTime SeedDate =
            new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static void Seed(ModelBuilder builder)
        {
            SeedRoles(builder);
            SeedUsers(builder);
            SeedUserRoles(builder);
            SeedMembers(builder);
            SeedShows(builder);
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

        private static void SeedUserRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = AdminRoleId,
                    UserId = AdminUserId
                }
            );
        }

        private static void SeedUsers(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = AdminUserId,
                UserName = AdminEmail,
                NormalizedUserName = AdminEmail.ToUpper(),
                Email = AdminEmail,
                NormalizedEmail = AdminEmail.ToUpper(),
                EmailConfirmed = true,
                DisplayName = $"{AdminFirstName} {AdminLastName}",
                ConcurrencyStamp = "static-user-stamp-001" // static — no spurious migrations
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Test1");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);
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
                    Instruments = new List<string> { "TO BE INSERTED" },
                    Bio = "Met een brede muzikale smaak en veel goesting om samen te spelen," +
                                    " is Katrientje Verhaert actief bij DOZ-band en Collective Memory XL." +
                                    " Gedreven door samenspel en muzikale nieuwsgierigheid zoekt ze steeds naar connectie in muziek.",
                    Image = "placeholder",
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
                    Bio = "TO BE INSERTED",
                    Image = "placeholder",
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
                                    " en nu een nieuwe uitdaging gevonden bij Collective Memory.",
                    Image = "placeholder",
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
                    Bio = "TO BE INSERTED",
                    Image = "placeholder",
                    CreatedAt = SeedDate
                },
                new Member
                {
                    Id = 5,
                    Firstname = "Bet",
                    Lastname = "Denayer",
                    Bands = new List<string> { "Collective Memory XL", "Black on light", "DOZ-band" },
                    FavoriteMusic = new List<string> { "Dave Matthews Band", "The Doors" },
                    Instruments = new List<string> { "Bass", "Zang" },
                    Bio = "TO BE INSERTED",
                    Image = "placeholder",
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
                    Bio = "TO BE INSERTED",
                    Image = "placeholder",
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
    }
}
