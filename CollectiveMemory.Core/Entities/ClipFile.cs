namespace CollectiveMemory.Core.Entities
{
    /// <summary>
    /// The bytes of an uploaded clip. Kept in its own table so listing clips never loads video data.
    /// </summary>
    public class ClipFile
    {
        public int ClipId { get; set; }
        public Clip Clip { get; set; } = null!;

        public string ContentType { get; set; } = string.Empty;
        public byte[] Data { get; set; } = [];
    }
}
