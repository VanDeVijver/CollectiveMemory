using CollectiveMemory.Core.Data;
using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CollectiveMemory.Core.Services
{
    public class ClipService : BaseService<Clip, int>, IClipService
    {
        public ClipService(ApplicationDbContext context,
            ILogger<BaseService<Clip, int>> logger) : base(context, logger)
        { }

        public Task<ClipFile?> GetVideoFileAsync(int clipId) =>
            _context.ClipFiles.AsNoTracking().FirstOrDefaultAsync(f => f.ClipId == clipId);
    }
}
