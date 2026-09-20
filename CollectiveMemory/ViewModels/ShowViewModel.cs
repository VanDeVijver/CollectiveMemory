namespace CollectiveMemory.ViewModels
{
    public class ShowViewModel
    {
        public int Id { get; set; }
        public string Venue { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? Street { get; set; }
        public string? StreetNumber { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string? AdditionalInfo { get; set; }
        public List<string>? AdditionalLinks { get; set; }

        // Computed display helpers
        public string DateDisplay => Date.ToString("MMM dd · yyyy");
        public string TimeDisplay => Date.ToString("HH:mm");
        public string PriceDisplay => Price == 0 ? "Free" : $"€{Price:F2}";
        public bool IsFree => Price == 0;
        public bool IsUpcoming => Date >= DateTime.UtcNow;
        public string? LocationDisplay
        {
            get
            {
                if (Street is null) return null;
                return StreetNumber is not null
                    ? $"{Street} {StreetNumber}"
                    : Street;
            }
        }
    }
    
    }
