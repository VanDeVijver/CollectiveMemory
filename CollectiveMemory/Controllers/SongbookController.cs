using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.Services.Interfaces;
using CollectiveMemory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectiveMemory.Controllers
{
    /// <summary>The band's repertoire. Band members only (never on the public site).</summary>
    [Authorize(Roles = "Admin")]
    [Route("Admin/Songbook")]
    public class SongbookController : Controller
    {
        private readonly ISongService _songService;

        public SongbookController(ISongService songService)
        {
            _songService = songService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var result = await _songService.GetAllAsync();
            if (!result.IsSuccess) TempData["AdminError"] = result.ErrorMessage;
            var usage = await _songService.GetSetlistCountsAsync();

            return View(new SongbookViewModel
            {
                Songs = (result.Data ?? [])
                    .OrderBy(s => s.Title, StringComparer.CurrentCultureIgnoreCase)
                    .Select(s => SongRowViewModel.From(s, usage.GetValueOrDefault(s.Id))).ToList()
            });
        }

        [HttpGet("New")]
        public IActionResult New() => FormView(new SongFormViewModel(), "New", null);

        [HttpPost("New")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(SongFormViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Title) && await _songService.TitleExistsAsync(model.Title))
                ModelState.AddModelError(nameof(model.Title), "That song is already in the songbook.");
            if (!ModelState.IsValid) return FormView(model, "New", null);

            var result = await _songService.CreateAsync(new Song
            {
                Title = model.Title.Trim(),
                Artist = Clean(model.Artist),
                Key = Clean(model.Key),
                Notes = Clean(model.Notes),
            });

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "The song could not be saved. Please try again.");
                return FormView(model, "New", null);
            }

            TempData["AdminMessage"] = $"Added: {model.Title.Trim()}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _songService.GetByIdAsync(id);
            if (!result.IsSuccess || result.Data is null) return NotFound();

            var s = result.Data;
            return FormView(new SongFormViewModel { Title = s.Title, Artist = s.Artist, Key = s.Key, Notes = s.Notes }, "Edit", id);
        }

        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SongFormViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Title) && await _songService.TitleExistsAsync(model.Title, exceptId: id))
                ModelState.AddModelError(nameof(model.Title), "Another song in the songbook already has that title.");
            if (!ModelState.IsValid) return FormView(model, "Edit", id);

            var existing = await _songService.GetByIdAsync(id);
            if (!existing.IsSuccess || existing.Data is null) return NotFound();

            var song = existing.Data;
            song.Title = model.Title.Trim();
            song.Artist = Clean(model.Artist);
            song.Key = Clean(model.Key);
            song.Notes = Clean(model.Notes);
            song.UpdatedAt = DateTime.UtcNow;

            var result = await _songService.UpdateAsync(song);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "The song could not be saved. Please try again.");
                return FormView(model, "Edit", id);
            }

            TempData["AdminMessage"] = $"Saved: {song.Title}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _songService.DeleteAsync(id);
            if (result.IsSuccess) TempData["AdminMessage"] = "Song removed from the songbook (and from any setlist it was in).";
            else TempData["AdminError"] = "That song could not be removed.";
            return RedirectToAction(nameof(Index));
        }

        private IActionResult FormView(SongFormViewModel model, string mode, int? id)
        {
            ViewData["Mode"] = mode;
            ViewData["SongId"] = id;
            return View("Form", model);
        }

        private static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
