namespace CollectiveMemory.Core.Entities
{
    /// <summary>A song placed at a position in one setlist.</summary>
    public class SetlistSong : BaseEntity
    {
        public int SetlistId { get; set; }
        public Setlist Setlist { get; set; } = null!;

        public int SongId { get; set; }
        public Song Song { get; set; } = null!;

        // Order within the setlist, starting at 1.
        public int Position { get; set; }
    }
}
