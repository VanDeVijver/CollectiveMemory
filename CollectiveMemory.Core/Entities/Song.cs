namespace CollectiveMemory.Core.Entities
{
    /// <summary>
    /// One song in the band's repertoire. The songbook is simply all of these; a song is written once
    /// and can be placed in any number of setlists.
    /// </summary>
    public class Song : BaseEntity
    {
        public string Title { get; set; } = string.Empty;

        // Who wrote or made it famous, e.g. "Crosby, Stills, Nash & Young".
        public string? Artist { get; set; }

        // The key the band plays it in, e.g. "Dm" or "G".
        public string? Key { get; set; }

        // Anything the band wants to remember: capo, tuning, who sings lead.
        public string? Notes { get; set; }

        // Original file name of the uploaded score (PDF or image), or null when the song has none.
        public string? ScoreFileName { get; set; }
        public SongScoreFile? ScoreFile { get; set; }

        public ICollection<SetlistSong> SetlistSongs { get; set; } = new List<SetlistSong>();
    }
}
