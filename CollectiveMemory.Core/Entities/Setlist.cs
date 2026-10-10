namespace CollectiveMemory.Core.Entities
{
    /// <summary>
    /// The planned songs for one show. Only band members see setlists (they are never part of the public site).
    /// A show may have no setlist; a setlist always belongs to exactly one show.
    /// </summary>
    public class Setlist : BaseEntity
    {
        public int ShowId { get; set; }
        public Show Show { get; set; } = null!;

        // The songs in play order (see SetlistSong.Position).
        public ICollection<SetlistSong> Songs { get; set; } = new List<SetlistSong>();
    }
}
