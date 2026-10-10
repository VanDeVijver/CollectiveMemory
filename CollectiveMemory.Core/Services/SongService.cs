using CollectiveMemory.Core.Data;
using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.ResultModels;
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

        public Task<SongScoreFile?> GetScoreFileAsync(int songId) =>
            _context.SongScoreFiles.AsNoTracking().FirstOrDefaultAsync(f => f.SongId == songId);

        public async Task<ResultModel<bool>> SetScoreAsync(int songId, string fileName, string contentType, byte[] data)
        {
            try
            {
                var song = await _context.Songs.FindAsync(songId);
                if (song is null) return ResultModel<bool>.Fail("That song no longer exists.");

                var file = await _context.SongScoreFiles.FindAsync(songId);
                if (file is null)
                    _context.SongScoreFiles.Add(new SongScoreFile { SongId = songId, ContentType = contentType, Data = data });
                else
                {
                    file.ContentType = contentType;
                    file.Data = data;
                }

                song.ScoreFileName = fileName;
                song.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return ResultModel<bool>.Ok(true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Score for song {SongId} could not be saved", songId);
                return ResultModel<bool>.Fail("The score could not be saved. Please try again.");
            }
        }

        public async Task<ResultModel<bool>> RemoveScoreAsync(int songId)
        {
            try
            {
                var song = await _context.Songs.FindAsync(songId);
                if (song is null) return ResultModel<bool>.Fail("That song no longer exists.");

                var file = await _context.SongScoreFiles.FindAsync(songId);
                if (file is not null) _context.SongScoreFiles.Remove(file);

                song.ScoreFileName = null;
                song.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return ResultModel<bool>.Ok(true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Score for song {SongId} could not be removed", songId);
                return ResultModel<bool>.Fail("The score could not be removed. Please try again.");
            }
        }

        public Task<Dictionary<int, int>> GetSetlistCountsAsync() =>
            _context.SetlistSongs.AsNoTracking()
                .GroupBy(x => x.SongId)
                .Select(g => new { SongId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.SongId, x => x.Count);
    }
}
