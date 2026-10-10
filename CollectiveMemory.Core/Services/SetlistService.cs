using CollectiveMemory.Core.Data;
using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.ResultModels;
using CollectiveMemory.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CollectiveMemory.Core.Services
{
    public class SetlistService : BaseService<Setlist, int>, ISetlistService
    {
        public SetlistService(ApplicationDbContext context,
            ILogger<BaseService<Setlist, int>> logger) : base(context, logger)
        { }

        public async Task<Setlist?> GetForShowAsync(int showId)
        {
            var setlist = await _context.Setlists.AsNoTracking()
                .Include(s => s.Songs).ThenInclude(x => x.Song)
                .FirstOrDefaultAsync(s => s.ShowId == showId);
            if (setlist is not null)
                setlist.Songs = setlist.Songs.OrderBy(x => x.Position).ThenBy(x => x.Id).ToList();
            return setlist;
        }

        public Task<Dictionary<int, int>> GetSongCountsByShowAsync() =>
            _context.Setlists.AsNoTracking()
                .Select(s => new { s.ShowId, Count = s.Songs.Count })
                .ToDictionaryAsync(x => x.ShowId, x => x.Count);

        public async Task<ResultModel<bool>> AddSongAsync(int showId, int songId)
        {
            try
            {
                if (!await _context.Shows.AnyAsync(s => s.Id == showId))
                    return ResultModel<bool>.Fail("That show no longer exists.");
                if (!await _context.Songs.AnyAsync(s => s.Id == songId))
                    return ResultModel<bool>.Fail("That song is no longer in the songbook.");

                var setlist = await _context.Setlists.Include(s => s.Songs)
                    .FirstOrDefaultAsync(s => s.ShowId == showId);
                if (setlist is null)
                {
                    setlist = new Setlist { ShowId = showId };
                    _context.Setlists.Add(setlist);
                }

                if (setlist.Songs.Any(x => x.SongId == songId))
                    return ResultModel<bool>.Fail("That song is already in this setlist.");

                var next = setlist.Songs.Count == 0 ? 1 : setlist.Songs.Max(x => x.Position) + 1;
                setlist.Songs.Add(new SetlistSong { SongId = songId, Position = next });

                await _context.SaveChangesAsync();
                return ResultModel<bool>.Ok(true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Song {SongId} could not be added to the setlist of show {ShowId}", songId, showId);
                return ResultModel<bool>.Fail("The song could not be added. Please try again.");
            }
        }

        public async Task<ResultModel<bool>> RemoveSongAsync(int setlistSongId)
        {
            try
            {
                var entry = await _context.SetlistSongs.FindAsync(setlistSongId);
                if (entry is null) return ResultModel<bool>.Fail("That song is no longer in the setlist.");

                _context.SetlistSongs.Remove(entry);
                await _context.SaveChangesAsync();

                // Close the gap so positions stay 1..n.
                var rest = await _context.SetlistSongs.Where(x => x.SetlistId == entry.SetlistId)
                    .OrderBy(x => x.Position).ToListAsync();
                for (var i = 0; i < rest.Count; i++) rest[i].Position = i + 1;
                await _context.SaveChangesAsync();

                return ResultModel<bool>.Ok(true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Setlist entry {Id} could not be removed", setlistSongId);
                return ResultModel<bool>.Fail("The song could not be removed. Please try again.");
            }
        }

        public async Task<ResultModel<bool>> MoveSongAsync(int setlistSongId, int direction)
        {
            try
            {
                var entry = await _context.SetlistSongs.FindAsync(setlistSongId);
                if (entry is null) return ResultModel<bool>.Fail("That song is no longer in the setlist.");

                var ordered = await _context.SetlistSongs.Where(x => x.SetlistId == entry.SetlistId)
                    .OrderBy(x => x.Position).ThenBy(x => x.Id).ToListAsync();

                var index = ordered.FindIndex(x => x.Id == setlistSongId);
                var target = index + Math.Sign(direction);
                if (target < 0 || target >= ordered.Count) return ResultModel<bool>.Ok(true);   // already at the end

                (ordered[index], ordered[target]) = (ordered[target], ordered[index]);
                for (var i = 0; i < ordered.Count; i++) ordered[i].Position = i + 1;

                await _context.SaveChangesAsync();
                return ResultModel<bool>.Ok(true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Setlist entry {Id} could not be moved", setlistSongId);
                return ResultModel<bool>.Fail("The song could not be moved. Please try again.");
            }
        }

        public async Task<ResultModel<bool>> DeleteForShowAsync(int showId)
        {
            try
            {
                var setlist = await _context.Setlists.Include(s => s.Songs)
                    .FirstOrDefaultAsync(s => s.ShowId == showId);
                if (setlist is null) return ResultModel<bool>.Ok(true);

                _context.Setlists.Remove(setlist);
                await _context.SaveChangesAsync();
                return ResultModel<bool>.Ok(true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Setlist of show {ShowId} could not be deleted", showId);
                return ResultModel<bool>.Fail("The setlist could not be deleted. Please try again.");
            }
        }
    }
}
