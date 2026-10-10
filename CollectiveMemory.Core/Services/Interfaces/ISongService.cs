using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.ResultModels;

namespace CollectiveMemory.Core.Services.Interfaces
{
    /// <summary>The songbook: every song in the band's repertoire.</summary>
    public interface ISongService : IBaseService<Song, int>
    {
        /// <summary>True if a song with this title (any capitalisation) already exists, other than <paramref name="exceptId"/>.</summary>
        Task<bool> TitleExistsAsync(string title, int? exceptId = null);

        /// <summary>How many setlists each song is in, by song id (songs in no setlist are absent).</summary>
        Task<Dictionary<int, int>> GetSetlistCountsAsync();

        /// <summary>The song's score bytes, or null if it has none.</summary>
        Task<SongScoreFile?> GetScoreFileAsync(int songId);

        /// <summary>Attaches a score to the song, replacing any previous one.</summary>
        Task<ResultModel<bool>> SetScoreAsync(int songId, string fileName, string contentType, byte[] data);

        /// <summary>Removes the song's score (not an error if it has none).</summary>
        Task<ResultModel<bool>> RemoveScoreAsync(int songId);
    }
}
