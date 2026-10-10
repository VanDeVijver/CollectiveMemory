namespace CollectiveMemory.ViewModels
{
    public class SetlistShowRowViewModel
    {
        public ShowViewModel Show { get; set; } = new();

        /// <summary>Null when the show has no setlist yet.</summary>
        public int? SongCount { get; set; }
    }

    public class SetlistsIndexViewModel
    {
        public List<SetlistShowRowViewModel> Upcoming { get; set; } = [];
        public List<SetlistShowRowViewModel> Past { get; set; } = [];
    }

    public class SetlistItemViewModel
    {
        /// <summary>Id of the placement (SetlistSong), used to move or remove it.</summary>
        public int Id { get; set; }
        public int Position { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Artist { get; set; }
        public string? Key { get; set; }
        public string? Notes { get; set; }
    }

    public class SetlistEditViewModel
    {
        public ShowViewModel Show { get; set; } = new();
        public bool HasSetlist { get; set; }
        public List<SetlistItemViewModel> Items { get; set; } = [];

        /// <summary>Songbook songs that are not in this setlist yet.</summary>
        public List<SongRowViewModel> Available { get; set; } = [];

        public bool SongbookIsEmpty { get; set; }
    }
}
