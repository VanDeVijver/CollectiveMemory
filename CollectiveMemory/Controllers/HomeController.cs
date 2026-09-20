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

                Shows = shows.Select(s => new ShowViewModel
                {
                    Id = s.Id,
                    Venue = s.Venue,
                    City = s.City,
                    Street = s.Street,
                    StreetNumber = s.StreetNumber,
                    Date = s.Date,
                    Price = s.Price,
                    AdditionalInfo = s.AdditionalInfo,
                    AdditionalLinks = s.AdditionalLinks,
                }).ToList()
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
                Shows = shows.Select(s => new ShowViewModel
                {
                    Id = s.Id,
                    Venue = s.Venue,
                    City = s.City,
                    Street = s.Street,
                    StreetNumber = s.StreetNumber,
                    Date = s.Date,
                    Price = s.Price,
                    AdditionalInfo = s.AdditionalInfo,
                    AdditionalLinks = s.AdditionalLinks,
                }).ToList()
            };
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
