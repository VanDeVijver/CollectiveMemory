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

        public AdminController(IShowService showService)
        {
            _showService = showService;
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
    }
}
