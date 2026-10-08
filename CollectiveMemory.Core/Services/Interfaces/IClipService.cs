using CollectiveMemory.Core.Entities;

namespace CollectiveMemory.Core.Services.Interfaces
{
    public interface IClipService : IBaseService<Clip, int>
    {
        /// <summary>The uploaded video bytes for a clip, or null if the clip has none.</summary>
        Task<ClipFile?> GetVideoFileAsync(int clipId);
    }
}
