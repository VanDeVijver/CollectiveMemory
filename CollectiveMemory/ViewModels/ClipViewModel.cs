using CollectiveMemory.Core.Entities;

namespace CollectiveMemory.ViewModels
{
    public class ClipViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string VideoUrl { get; set; } = string.Empty;
        public string? PosterUrl { get; set; }

        public string? YouTubeEmbedUrl => VideoLink.TryYouTube(VideoUrl, out var embed) ? embed : null;

        public static ClipViewModel From(Clip c) => new()
        {
            Id = c.Id,
            Title = c.Title,
            Subtitle = c.Subtitle,
            VideoUrl = c.HasUploadedVideo ? $"/clips/{c.Id}/video" : c.VideoUrl,
            PosterUrl = c.PosterUrl,
        };
    }
}
