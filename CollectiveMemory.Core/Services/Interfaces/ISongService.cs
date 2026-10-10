using CollectiveMemory.Core.Entities;

namespace CollectiveMemory.Core.Services.Interfaces
{
    /// <summary>The songbook: every song in the band's repertoire.</summary>
    public interface ISongService : IBaseService<Song, int>
    {
        /// <summary>True if a song with this title (any capitalisation) already exists, other than <paramref name="exceptId"/>.</summary>
        Task<bool> TitleExistsAsync(string title, int? exceptId = null);

        /// <summary>How many setlists each song is in, by song id (songs in no setlist are absent).</summary>
        Task<Dictionary<int, int>> GetSetlistCountsAsync();
    }
}
