using CollectiveMemory.Core.Data;
using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CollectiveMemory.Core.Services
{
    public class SongService : BaseService<Song, int>, ISongService
    {
        public SongService(ApplicationDbContext context,
            ILogger<BaseService<Song, int>> logger) : base(context, logger)
        { }

        public Task<bool> TitleExistsAsync(string title, int? exceptId = null)
        {
            var wanted = (title ?? string.Empty).Trim().ToLower();
            if (wanted.Length == 0) return Task.FromResult(false);
            return _context.Songs.AsNoTracking()
                .AnyAsync(s => s.Title.ToLower() == wanted && (exceptId == null || s.Id != exceptId));
        }

        public Task<Dictionary<int, int>> GetSetlistCountsAsync() =>
            _context.SetlistSongs.AsNoTracking()
                .GroupBy(x => x.SongId)
                .Select(g => new { SongId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.SongId, x => x.Count);
    }
}
