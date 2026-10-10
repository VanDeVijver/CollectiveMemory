using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.ResultModels;

namespace CollectiveMemory.Core.Services.Interfaces
{
    public interface ISetlistService : IBaseService<Setlist, int>
    {
        /// <summary>The show's setlist with its songs in play order, or null if the show has none.</summary>
        Task<Setlist?> GetForShowAsync(int showId);

        /// <summary>Number of songs per show, by show id (shows without a setlist are absent).</summary>
        Task<Dictionary<int, int>> GetSongCountsByShowAsync();

        /// <summary>Appends a song to the show's setlist, creating the setlist first if the show has none.</summary>
        Task<ResultModel<bool>> AddSongAsync(int showId, int songId);

        /// <summary>Removes one entry and closes the gap in the numbering.</summary>
        Task<ResultModel<bool>> RemoveSongAsync(int setlistSongId);

        /// <summary>Moves an entry one place up (-1) or down (+1).</summary>
        Task<ResultModel<bool>> MoveSongAsync(int setlistSongId, int direction);

        /// <summary>Deletes the show's setlist (the songs stay in the songbook).</summary>
        Task<ResultModel<bool>> DeleteForShowAsync(int showId);
    }
}
