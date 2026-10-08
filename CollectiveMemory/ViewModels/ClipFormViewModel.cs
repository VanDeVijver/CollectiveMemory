using System.ComponentModel.DataAnnotations;

namespace CollectiveMemory.ViewModels
{
    public class ClipFormViewModel : IValidatableObject
    {
        // Videos are stored in the database, so keep uploads small (a 15-30 s clip is ~1-3 MB).
        public const long MaxVideoBytes = 25 * 1024 * 1024;

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200), Display(Name = "Subtitle")]
        public string? Subtitle { get; set; }

        [Display(Name = "Video file")]
        public IFormFile? VideoFile { get; set; }

        [StringLength(500), Display(Name = "Video link")]
        public string? VideoUrl { get; set; }

        [StringLength(500), Display(Name = "Preview image link")]
        public string? PosterUrl { get; set; }

        /// <summary>Set by validation when a file passes the content check.</summary>
        public string? VideoContentType { get; private set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var hasFile = VideoFile is { Length: > 0 };
            var hasLink = !string.IsNullOrWhiteSpace(VideoUrl);

            if (hasFile == hasLink)
            {
                yield return new ValidationResult(
                    hasFile ? "Choose a video file or paste a link, not both."
                            : "Choose a video file or paste a link.",
                    [nameof(VideoFile), nameof(VideoUrl)]);
                yield break;
            }

            if (hasFile)
            {
                if (VideoFile!.Length > MaxVideoBytes)
                {
                    yield return new ValidationResult(
                        $"That file is {VideoFile.Length / 1048576.0:0.#} MB. The limit is {MaxVideoBytes / 1048576} MB: " +
                        "cut the video to 15-30 seconds or compress it first.", [nameof(VideoFile)]);
                }
                else
                {
                    var head = new byte[12];
                    using (var stream = VideoFile.OpenReadStream())
                        stream.ReadExactly(head, 0, (int)Math.Min(head.Length, VideoFile.Length));

                    VideoContentType = VideoLink.SniffVideoType(head);
                    if (VideoContentType is null)
                    {
                        yield return new ValidationResult(
                            "That doesn't look like an .mp4 or .webm video.", [nameof(VideoFile)]);
                    }
                }
            }
            else if (!VideoLink.TryYouTube(VideoUrl, out _) && !VideoLink.IsVideoFile(VideoUrl))
            {
                yield return new ValidationResult(
                    "Use a YouTube link, or an https link to an .mp4 or .webm file.", [nameof(VideoUrl)]);
            }

            if (!string.IsNullOrWhiteSpace(PosterUrl)
                && !(Uri.TryCreate(PosterUrl, UriKind.Absolute, out var poster) && poster.Scheme == Uri.UriSchemeHttps))
            {
                yield return new ValidationResult(
                    "The preview image must be an https link.", [nameof(PosterUrl)]);
            }
        }
    }
}
