
using CollectiveMemory.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CollectiveMemory.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Show> Shows => Set<Show>();
        public DbSet<Member> Members => Set<Member>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Member>(entity =>
            {
                entity.Property(e => e.Bands).HasColumnType("text[]");
                entity.Property(e => e.FavoriteMusic).HasColumnType("text[]");
                entity.Property(e => e.Instruments).HasColumnType("text[]");
            });

            builder.Entity<Show>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Venue).IsRequired().HasMaxLength(200);
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Street).HasMaxLength(200);
                entity.Property(e => e.StreetNumber).HasMaxLength(20);
                entity.Property(e => e.AdditionalLinks).HasColumnType("text[]");
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
            });

            DataSeeder.Seed(builder);
        }


    }
}
