using System.ComponentModel.DataAnnotations;

namespace CollectiveMemory.ViewModels
{
    public class ShowFormViewModel : IValidatableObject
    {
        [Required, StringLength(200)]
        public string Venue { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string City { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Street { get; set; }

        [StringLength(20), Display(Name = "Number")]
        public string? StreetNumber { get; set; }

        // Entered and displayed as the clock time on the poster, same as the existing shows.
        [Required, DataType(DataType.DateTime), Display(Name = "Date & start time")]
        public DateTime Date { get; set; } = DateTime.Today.AddDays(1).AddHours(20);

        [Range(0, 1000, ErrorMessage = "Enter a price between 0 and 1000 (0 = free).")]
        [Display(Name = "Price (€)")]
        public double Price { get; set; }

        [StringLength(1000), Display(Name = "Extra info")]
        public string? AdditionalInfo { get; set; }

        [Display(Name = "Links (one per line)")]
        public string? LinksText { get; set; }

        public List<string> Links => (LinksText ?? string.Empty)
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        // Links are rendered as <a href>, so only allow real http(s) URLs (no javascript: etc.).
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var link in Links)
            {
                if (!Uri.TryCreate(link, UriKind.Absolute, out var uri)
                    || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    yield return new ValidationResult(
                        $"\"{link}\" is not a valid link. Start it with http:// or https://",
                        [nameof(LinksText)]);
                }
            }
        }
    }
}
