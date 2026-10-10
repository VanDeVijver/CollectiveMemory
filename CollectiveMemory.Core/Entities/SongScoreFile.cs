namespace CollectiveMemory.Core.Entities
{
    /// <summary>
    /// The bytes of a song's score (PDF or image). Kept in its own table so listing the songbook never
    /// loads file data. A song has at most one score.
    /// </summary>
    public class SongScoreFile
    {
        public int SongId { get; set; }
        public Song Song { get; set; } = null!;

        public string ContentType { get; set; } = string.Empty;
        public byte[] Data { get; set; } = [];
    }
}
