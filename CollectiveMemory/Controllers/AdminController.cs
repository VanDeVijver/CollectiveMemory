using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.Services.Interfaces;
using CollectiveMemory.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollectiveMemory.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly IShowService _showService;
        private readonly IClipService _clipService;

        public AdminController(IShowService showService, IClipService clipService)
        {
            _showService = showService;
            _clipService = clipService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var result = await _showService.GetAllAsync();
            if (!result.IsSuccess) TempData["AdminError"] = result.ErrorMessage;

            var today = DateTime.UtcNow.Date;
            var shows = (result.Data ?? []).Select(ShowViewModel.From).ToList();

            return View(new AdminShowsViewModel
            {
                Upcoming = shows.Where(s => s.Date.Date >= today).OrderBy(s => s.Date).ToList(),
                Past = shows.Where(s => s.Date.Date < today).OrderByDescending(s => s.Date).ToList(),
            });
        }

        [HttpGet("Shows/New")]
        public IActionResult Create() => View(new ShowFormViewModel());

        [HttpPost("Shows/New")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShowFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var links = model.Links;
            var result = await _showService.CreateAsync(new Show
            {
                Venue = model.Venue.Trim(),
                City = model.City.Trim(),
                Street = model.Street?.Trim(),
                StreetNumber = model.StreetNumber?.Trim(),
                // Stored as entered (no time-zone shift) so the site shows the time typed here.
                Date = DateTime.SpecifyKind(model.Date, DateTimeKind.Utc),
                Price = model.Price,
                AdditionalInfo = model.AdditionalInfo?.Trim(),
                AdditionalLinks = links.Count > 0 ? links : null,
            });

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "The show could not be saved. Please try again.");
                return View(model);
            }

            TempData["AdminMessage"] = $"Added: {model.Venue.Trim()}, {model.City.Trim()}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Shows/Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _showService.DeleteAsync(id);
            if (result.IsSuccess) TempData["AdminMessage"] = "Show removed.";
            else TempData["AdminError"] = "That show could not be removed.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Clips")]
        public async Task<IActionResult> Clips()
        {
            var result = await _clipService.GetAllAsync();
            if (!result.IsSuccess) TempData["AdminError"] = result.ErrorMessage;

            return View(new AdminClipsViewModel
            {
                Clips = (result.Data ?? [])
                    .OrderByDescending(c => c.CreatedAt).ThenBy(c => c.Id)
                    .Select(ClipViewModel.From).ToList()
            });
        }

        [HttpGet("Clips/New")]
        public IActionResult CreateClip() => View(new ClipFormViewModel());

        [HttpPost("Clips/New")]
        [ValidateAntiForgeryToken]
        // Room for the 25 MB video plus the other form fields.
        [RequestSizeLimit(ClipFormViewModel.MaxVideoBytes + 1_000_000)]
        [RequestFormLimits(MultipartBodyLengthLimit = ClipFormViewModel.MaxVideoBytes + 1_000_000)]
        public async Task<IActionResult> CreateClip(ClipFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var clip = new Clip
            {
                Title = model.Title.Trim(),
                Subtitle = model.Subtitle?.Trim(),
                PosterUrl = string.IsNullOrWhiteSpace(model.PosterUrl) ? null : model.PosterUrl.Trim(),
            };

            if (model.VideoFile is { Length: > 0 } upload)
            {
                using var buffer = new MemoryStream((int)upload.Length);
                await upload.CopyToAsync(buffer);
                clip.HasUploadedVideo = true;
                clip.VideoFile = new ClipFile { ContentType = model.VideoContentType!, Data = buffer.ToArray() };
            }
            else
            {
                clip.VideoUrl = model.VideoUrl!.Trim();
            }

            var result = await _clipService.CreateAsync(clip);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, "The clip could not be saved. Please try again.");
                return View(model);
            }

            TempData["AdminMessage"] = $"Added: {model.Title.Trim()}.";
            return RedirectToAction(nameof(Clips));
        }

        [HttpPost("Clips/Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClip(int id)
        {
            var result = await _clipService.DeleteAsync(id);
            if (result.IsSuccess) TempData["AdminMessage"] = "Clip removed.";
            else TempData["AdminError"] = "That clip could not be removed.";
            return RedirectToAction(nameof(Clips));
        }
    }
}
