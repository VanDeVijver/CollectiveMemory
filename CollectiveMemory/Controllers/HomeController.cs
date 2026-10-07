using System.Diagnostics;
using CollectiveMemory.Core.Services;
using CollectiveMemory.Core.Services.Interfaces;
using CollectiveMemory.Models;
using CollectiveMemory.ViewModels;
using Microsoft.AspNetCore.Mvc;
using CollectiveMemory.Core.Entities;

namespace CollectiveMemory.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IShowService _showService;
        private readonly IMemberService _memberService;

        public HomeController(IShowService showService, IMemberService memberService,ILogger<HomeController> logger)
        {
            _logger = logger;
            _showService = showService;
            _memberService = memberService;
        }

        public async Task<IActionResult> Index()
        {
            var membersResult = await _memberService.GetAllAsync();
            var showsResult = await _showService.GetAllAsync();

            var members = membersResult.Data;
            var shows = showsResult.Data;

            if (!membersResult.IsSuccess)
            {
                _logger.LogWarning("Leden konden niet worden opgehaald: {Error}"
                    , membersResult.ErrorMessage);
                TempData["Error"] = membersResult.ErrorMessage;
            }
            if (!showsResult.IsSuccess)
            {
                _logger.LogWarning("Shows konden niet worden opgehaald: {Error}", showsResult.ErrorMessage);
                TempData["Error"] = showsResult.ErrorMessage;
            }


            var vm = new HomeIndexViewModel
            {
                Members = members.Select(m => new MemberViewModel
                {
                    Id = m.Id,
                    Firstname = m.Firstname,
                    Lastname = m.Lastname,
                    Bands = m.Bands,
                    FavoriteMusic = m.FavoriteMusic,
                    Instruments = m.Instruments,
                    Bio = m.Bio,
                    Image = m.Image,
                }).ToList(),

                Shows = UpcomingShows(shows)
            };


            return View(vm);
        }

        public async Task<IActionResult> Shows()
        {
            var showsResult = await _showService.GetAllAsync();

            var shows = showsResult.Data;

            if (!showsResult.IsSuccess)
            {
                _logger.LogWarning("Shows konden niet worden opgehaald: {Error}", showsResult.ErrorMessage);
                TempData["Error"] = showsResult.ErrorMessage;
            }

            var vm = new ShowsViewModel
            {
                Shows = UpcomingShows(shows)
            };
            return View(vm);
        }

        // Public pages only list shows that haven't happened yet, soonest first.
        private static List<ShowViewModel> UpcomingShows(IEnumerable<Show>? shows) =>
            (shows ?? [])
                .Where(s => s.Date.Date >= DateTime.UtcNow.Date)
                .OrderBy(s => s.Date)
                .Select(ShowViewModel.From)
                .ToList();

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Target of UseStatusCodePagesWithReExecute; the original status code is preserved.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult HttpStatus(int id)
        {
            if (id == 404) return View("NotFound");
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
