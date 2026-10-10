using CollectiveMemory.Core.Services.Interfaces;
using CollectiveMemory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectiveMemory.Controllers
{
    /// <summary>Setlists per show. Band members only (never on the public site).</summary>
    [Authorize(Roles = "Admin")]
    [Route("Admin/Setlists")]
    public class SetlistsController : Controller
    {
        private readonly IShowService _showService;
        private readonly ISetlistService _setlistService;
        private readonly ISongService _songService;

        public SetlistsController(IShowService showService, ISetlistService setlistService, ISongService songService)
        {
            _showService = showService;
            _setlistService = setlistService;
            _songService = songService;
        }

        // Every show, with how many songs its setlist has (or none yet)
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var shows = await _showService.GetAllAsync();
            if (!shows.IsSuccess) TempData["AdminError"] = shows.ErrorMessage;
            var counts = await _setlistService.GetSongCountsByShowAsync();

            var today = DateTime.UtcNow.Date;
            var rows = (shows.Data ?? []).Select(s => new SetlistShowRowViewModel
            {
                Show = ShowViewModel.From(s),
                SongCount = counts.TryGetValue(s.Id, out var n) ? n : null,
            }).ToList();

            return View(new SetlistsIndexViewModel
            {
                Upcoming = rows.Where(r => r.Show.Date.Date >= today).OrderBy(r => r.Show.Date).ToList(),
                Past = rows.Where(r => r.Show.Date.Date < today).OrderByDescending(r => r.Show.Date).ToList(),
            });
        }

        [HttpGet("{showId:int}")]
        public async Task<IActionResult> Edit(int showId)
        {
            var show = await _showService.GetByIdAsync(showId);
            if (!show.IsSuccess || show.Data is null) return NotFound();

            var setlist = await _setlistService.GetForShowAsync(showId);
            var items = (setlist?.Songs ?? []).Select(x => new SetlistItemViewModel
            {
                Id = x.Id, Position = x.Position, Title = x.Song.Title,
                Artist = x.Song.Artist, Key = x.Song.Key, Notes = x.Song.Notes,
            }).ToList();

            var songbook = (await _songService.GetAllAsync()).Data ?? [];
            var used = (setlist?.Songs ?? []).Select(x => x.SongId).ToHashSet();

            return View(new SetlistEditViewModel
            {
                Show = ShowViewModel.From(show.Data),
                HasSetlist = setlist is not null,
                Items = items,
                SongbookIsEmpty = !songbook.Any(),
                Available = songbook.Where(s => !used.Contains(s.Id))
                    .OrderBy(s => s.Title, StringComparer.CurrentCultureIgnoreCase)
                    .Select(s => SongRowViewModel.From(s)).ToList(),
            });
        }

        [HttpPost("{showId:int}/Songs")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSong(int showId, int songId)
        {
            var result = await _setlistService.AddSongAsync(showId, songId);
            if (!result.IsSuccess) TempData["AdminError"] = result.ErrorMessage;
            return RedirectToAction(nameof(Edit), new { showId });
        }

        [HttpPost("{showId:int}/Songs/{id:int}/Move")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Move(int showId, int id, int direction)
        {
            var result = await _setlistService.MoveSongAsync(id, direction);
            if (!result.IsSuccess) TempData["AdminError"] = result.ErrorMessage;
            return RedirectToAction(nameof(Edit), new { showId });
        }

        [HttpPost("{showId:int}/Songs/{id:int}/Remove")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSong(int showId, int id)
        {
            var result = await _setlistService.RemoveSongAsync(id);
            if (!result.IsSuccess) TempData["AdminError"] = result.ErrorMessage;
            return RedirectToAction(nameof(Edit), new { showId });
        }

        [HttpPost("{showId:int}/Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSetlist(int showId)
        {
            var result = await _setlistService.DeleteForShowAsync(showId);
            TempData[result.IsSuccess ? "AdminMessage" : "AdminError"] =
                result.IsSuccess ? "Setlist deleted. The songs are still in the songbook." : result.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
    }
}
