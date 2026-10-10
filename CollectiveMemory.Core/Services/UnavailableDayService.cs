using CollectiveMemory.Core.Data;
using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.ResultModels;
using CollectiveMemory.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CollectiveMemory.Core.Services
{
    public class UnavailableDayService : BaseService<UnavailableDay, int>, IUnavailableDayService
    {
        public UnavailableDayService(ApplicationDbContext context,
            ILogger<BaseService<UnavailableDay, int>> logger) : base(context, logger)
        { }

        public Task<List<UnavailableDay>> GetRangeAsync(DateOnly from, DateOnly to) =>
            _context.UnavailableDays.AsNoTracking()
                .Where(d => d.Date >= from && d.Date <= to)
                .OrderBy(d => d.Date).ToListAsync();

        public Task<List<UnavailableDay>> GetUpcomingAsync(DateOnly from, int take) =>
            _context.UnavailableDays.AsNoTracking()
                .Where(d => d.Date >= from)
                .OrderBy(d => d.Date).Take(take).ToListAsync();

        public async Task<ResultModel<bool>> SetAsync(DateOnly date, string? note)
        {
            try
            {
                // A show's date can't be marked; checked here as well as in the UI.
                var dayStart = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
                var dayEnd = dayStart.AddDays(1);
                if (await _context.Shows.AnyAsync(s => s.Date >= dayStart && s.Date < dayEnd))
                    return ResultModel<bool>.Fail("There is a show that day, so it can't be marked.");

                note = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
                var existing = await _context.UnavailableDays.FirstOrDefaultAsync(d => d.Date == date);
                if (existing is null)
                {
                    _context.UnavailableDays.Add(new UnavailableDay { Date = date, Note = note });
                }
                else
                {
                    existing.Note = note;
                    existing.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                return ResultModel<bool>.Ok(true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Day {Date} could not be marked", date);
                return ResultModel<bool>.Fail("The day could not be saved. Please try again.");
            }
        }

        public async Task<ResultModel<bool>> ClearAsync(DateOnly date)
        {
            try
            {
                var existing = await _context.UnavailableDays.FirstOrDefaultAsync(d => d.Date == date);
                if (existing is not null)
                {
                    _context.UnavailableDays.Remove(existing);
                    await _context.SaveChangesAsync();
                }
                return ResultModel<bool>.Ok(true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Day {Date} could not be cleared", date);
                return ResultModel<bool>.Fail("The day could not be cleared. Please try again.");
            }
        }
    }
}
