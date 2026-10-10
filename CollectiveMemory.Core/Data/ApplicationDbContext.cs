
using CollectiveMemory.Core.Entities;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CollectiveMemory.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDataProtectionKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Show> Shows => Set<Show>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Clip> Clips => Set<Clip>();
        public DbSet<ClipFile> ClipFiles => Set<ClipFile>();
        public DbSet<Setlist> Setlists => Set<Setlist>();
        public DbSet<Song> Songs => Set<Song>();
        public DbSet<SetlistSong> SetlistSongs => Set<SetlistSong>();

        // Auth cookie/antiforgery keys live in the database so they survive container restarts.
        public DbSet<DataProtectionKey> DataProtectionKeys => Set<DataProtectionKey>();

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

            builder.Entity<Clip>(entity =>
            {
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Subtitle).HasMaxLength(200);
                entity.Property(e => e.VideoUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.PosterUrl).HasMaxLength(500);
            });

            builder.Entity<ClipFile>(entity =>
            {
                entity.HasKey(e => e.ClipId);
                entity.Property(e => e.ContentType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Data).IsRequired();
                entity.HasOne(e => e.Clip).WithOne(c => c.VideoFile)
                    .HasForeignKey<ClipFile>(e => e.ClipId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Setlist>(entity =>
            {
                // 1 : 0..1 with Show. The FK is unique, and deleting a show deletes its setlist.
                entity.HasOne(e => e.Show).WithOne(s => s.Setlist)
                    .HasForeignKey<Setlist>(e => e.ShowId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Song>(entity =>
            {
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Artist).HasMaxLength(200);
                entity.Property(e => e.Key).HasMaxLength(20);
                entity.Property(e => e.Notes).HasMaxLength(500);
            });

            builder.Entity<SetlistSong>(entity =>
            {
                // Removing a setlist, or a song from the songbook, removes the placements too.
                entity.HasOne(e => e.Setlist).WithMany(s => s.Songs)
                    .HasForeignKey(e => e.SetlistId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Song).WithMany(s => s.SetlistSongs)
                    .HasForeignKey(e => e.SongId).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(e => new { e.SetlistId, e.SongId }).IsUnique();   // a song once per setlist
                entity.HasIndex(e => new { e.SetlistId, e.Position });
            });

            DataSeeder.Seed(builder);
        }


    }
}
