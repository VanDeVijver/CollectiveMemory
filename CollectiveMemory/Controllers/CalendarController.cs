using System.Globalization;
using CollectiveMemory.Core.Services.Interfaces;
using CollectiveMemory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectiveMemory.Controllers
{
    /// <summary>
    /// The band's rehearsal calendar. Everyone shares one login, so a day is either free or marked
    /// "can't rehearse" (if one member can't, nobody can). Show days are shown but can't be marked.
    /// Band members only.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Route("Admin/Calendar")]
    public class CalendarController : Controller
    {
        private static readonly CultureInfo English = CultureInfo.GetCultureInfo("en-GB");

        private readonly IUnavailableDayService _days;
        private readonly IShowService _shows;

        public CalendarController(IUnavailableDayService days, IShowService shows)
        {
            _days = days;
            _shows = shows;
        }

        // ?month=2026-11 shows that month, ?day=2026-11-12 opens that day's panel
        [HttpGet("")]
        public async Task<IActionResult> Index(string? month, string? day)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var first = ParseMonth(month) ?? new DateOnly(today.Year, today.Month, 1);

            // whole weeks, Monday first
            var gridStart = first.AddDays(-(((int)first.DayOfWeek + 6) % 7));
            var last = first.AddMonths(1).AddDays(-1);
            var gridEnd = last.AddDays(6 - (((int)last.DayOfWeek + 6) % 7));

            var blocked = (await _days.GetRangeAsync(gridStart, gridEnd)).ToDictionary(d => d.Date);

            var showsResult = await _shows.GetAllAsync();
            if (!showsResult.IsSuccess) TempData["AdminError"] = showsResult.ErrorMessage;
            var showsByDay = (showsResult.Data ?? [])
                .GroupBy(s => DateOnly.FromDateTime(s.Date))
                .Where(g => g.Key >= gridStart && g.Key <= gridEnd)
                .ToDictionary(g => g.Key, g => ShowViewModel.From(g.OrderBy(s => s.Date).First()));

            DateOnly? selectedDate = DateOnly.TryParseExact(day, "yyyy-MM-dd", out var parsed) ? parsed : null;

            var weeks = new List<List<CalendarDayViewModel>>();
            CalendarDayViewModel? selected = null;
            for (var d = gridStart; d <= gridEnd; d = d.AddDays(7))
            {
                var week = new List<CalendarDayViewModel>();
                for (var i = 0; i < 7; i++)
                {
                    var date = d.AddDays(i);
                    blocked.TryGetValue(date, out var entry);
                    showsByDay.TryGetValue(date, out var show);
                    var cell = new CalendarDayViewModel
                    {
                        Date = date,
                        InMonth = date.Month == first.Month,
                        IsToday = date == today,
                        IsPast = date < today,
                        IsBlocked = entry is not null,
                        Note = entry?.Note,
                        Show = show,
                    };
                    cell.IsSelected = cell.Selectable && selectedDate == date;
                    if (cell.IsSelected) selected = cell;
                    week.Add(cell);
                }
                weeks.Add(week);
            }

            var upcoming = await _days.GetUpcomingAsync(today, 12);

            return View(new CalendarViewModel
            {
                Title = first.ToString("MMMM yyyy", English),
                Month = MonthKey(first),
                PrevMonth = MonthKey(first.AddMonths(-1)),
                NextMonth = MonthKey(first.AddMonths(1)),
                ThisMonth = MonthKey(new DateOnly(today.Year, today.Month, 1)),
                Weeks = weeks,
                Selected = selected,
                Upcoming = upcoming.Select(u => new BlockedDayViewModel { Date = u.Date, Note = u.Note }).ToList(),
            });
        }

        [HttpPost("Set")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Set(DateOnly date, string? note, string? month)
        {
            if (date < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                TempData["AdminError"] = "That day has already passed.";
                return Back(month);
            }

            var result = await _days.SetAsync(date, note is { Length: > 200 } ? note[..200] : note);
            TempData[result.IsSuccess ? "AdminMessage" : "AdminError"] = result.IsSuccess
                ? $"Marked {date.ToString("dddd d MMMM", English)}: the band can't rehearse."
                : result.ErrorMessage;
            return Back(month);
        }

        [HttpPost("Clear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clear(DateOnly date, string? month)
        {
            var result = await _days.ClearAsync(date);
            TempData[result.IsSuccess ? "AdminMessage" : "AdminError"] = result.IsSuccess
                ? $"{date.ToString("dddd d MMMM", English)} is free again."
                : result.ErrorMessage;
            return Back(month);
        }

        private IActionResult Back(string? month) =>
            RedirectToAction(nameof(Index), new { month = ParseMonth(month) is { } m ? MonthKey(m) : null });

        private static string MonthKey(DateOnly d) => d.ToString("yyyy-MM", CultureInfo.InvariantCulture);

        private static DateOnly? ParseMonth(string? value) =>
            DateOnly.TryParseExact(value + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)
                ? d : null;
    }
}
