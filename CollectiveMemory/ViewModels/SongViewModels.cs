using System.ComponentModel.DataAnnotations;
using CollectiveMemory.Core.Entities;

namespace CollectiveMemory.ViewModels
{
    public class SongFormViewModel
    {
        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Artist { get; set; }

        [StringLength(20), Display(Name = "Key")]
        public string? Key { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class SongRowViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Artist { get; set; }
        public string? Key { get; set; }
        public string? Notes { get; set; }
        public int SetlistCount { get; set; }

        public static SongRowViewModel From(Song s, int setlistCount = 0) => new()
        {
            Id = s.Id, Title = s.Title, Artist = s.Artist, Key = s.Key, Notes = s.Notes, SetlistCount = setlistCount,
        };
    }

    public class SongbookViewModel
    {
        public List<SongRowViewModel> Songs { get; set; } = [];
    }
}
