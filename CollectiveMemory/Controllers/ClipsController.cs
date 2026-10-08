using CollectiveMemory.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CollectiveMemory.Controllers
{
    /// <summary>Serves clips that were uploaded through the admin (stored in the database).</summary>
    public class ClipsController : Controller
    {
        private readonly IClipService _clipService;

        public ClipsController(IClipService clipService)
        {
            _clipService = clipService;
        }

        [HttpGet("/clips/{id:int}/video")]
        public async Task<IActionResult> Video(int id)
        {
            var file = await _clipService.GetVideoFileAsync(id);
            if (file is null) return NotFound();

            // Clip ids are never reused, so the bytes behind a URL never change.
            Response.Headers.CacheControl = "public, max-age=604800, immutable";
            Response.Headers["X-Content-Type-Options"] = "nosniff";
            return File(file.Data, file.ContentType, enableRangeProcessing: true);
        }
    }
}
