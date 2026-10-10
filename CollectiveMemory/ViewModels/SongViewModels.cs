using System.ComponentModel.DataAnnotations;
using CollectiveMemory.Core.Entities;

namespace CollectiveMemory.ViewModels
{
    public class SongFormViewModel : IValidatableObject
    {
        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Artist { get; set; }

        [StringLength(20), Display(Name = "Key")]
        public string? Key { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        // Optional score: a PDF or an image. On edit, a new file replaces the current one.
        [Display(Name = "Score")]
        public IFormFile? ScoreFile { get; set; }

        public bool RemoveScore { get; set; }

        /// <summary>Shown on the edit form; not posted.</summary>
        public string? CurrentScoreFileName { get; set; }

        /// <summary>Set by validation when an uploaded score passes the content check.</summary>
        public string? ScoreContentType { get; private set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ScoreFile is not { Length: > 0 }) yield break;

            if (ScoreFile.Length > ScoreFileType.MaxBytes)
            {
                yield return new ValidationResult(
                    $"That file is {ScoreFile.Length / 1048576.0:0.#} MB. The limit is {ScoreFileType.MaxBytes / 1048576} MB.",
                    [nameof(ScoreFile)]);
                yield break;
            }

            var head = new byte[8];
            using (var stream = ScoreFile.OpenReadStream())
                stream.ReadExactly(head, 0, (int)Math.Min(head.Length, ScoreFile.Length));

            ScoreContentType = ScoreFileType.Sniff(head);
            if (ScoreContentType is null)
                yield return new ValidationResult("The score must be a PDF, PNG or JPEG file.", [nameof(ScoreFile)]);
        }
    }

    public class SongRowViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Artist { get; set; }
        public string? Key { get; set; }
        public string? Notes { get; set; }
        public int SetlistCount { get; set; }
        public string? ScoreFileName { get; set; }
        public bool HasScore => ScoreFileName is not null;

        public static SongRowViewModel From(Song s, int setlistCount = 0) => new()
        {
            Id = s.Id, Title = s.Title, Artist = s.Artist, Key = s.Key, Notes = s.Notes, SetlistCount = setlistCount,
            ScoreFileName = s.ScoreFileName,
        };
    }

    public class SongbookViewModel
    {
        public List<SongRowViewModel> Songs { get; set; } = [];
    }
}
