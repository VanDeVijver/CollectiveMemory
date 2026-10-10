namespace CollectiveMemory.ViewModels
{
    public class CalendarDayViewModel
    {
        public DateOnly Date { get; set; }
        public bool InMonth { get; set; }
        public bool IsToday { get; set; }
        public bool IsPast { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsSelected { get; set; }
        public string? Note { get; set; }

        /// <summary>Set when there is a show that day. Such a day can't be marked.</summary>
        public ShowViewModel? Show { get; set; }

        public bool Selectable => !IsPast && Show is null;
    }

    public class BlockedDayViewModel
    {
        public DateOnly Date { get; set; }
        public string? Note { get; set; }
    }

    public class CalendarViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;        // yyyy-MM, the month being shown
        public string PrevMonth { get; set; } = string.Empty;
        public string NextMonth { get; set; } = string.Empty;
        public string ThisMonth { get; set; } = string.Empty;

        public List<List<CalendarDayViewModel>> Weeks { get; set; } = [];
        public CalendarDayViewModel? Selected { get; set; }
        public List<BlockedDayViewModel> Upcoming { get; set; } = [];
    }
}
