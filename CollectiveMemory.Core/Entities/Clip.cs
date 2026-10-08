namespace CollectiveMemory.Core.Entities
{
    public class Clip : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }

        // A YouTube link, or a direct link to an .mp4/.webm file. Empty for uploaded clips.
        public string VideoUrl { get; set; } = string.Empty;

        // True when the video was uploaded from a PC and lives in ClipFile.
        public bool HasUploadedVideo { get; set; }
        public ClipFile? VideoFile { get; set; }

        // Optional preview image, only used for direct video files.
        public string? PosterUrl { get; set; }
    }
}
