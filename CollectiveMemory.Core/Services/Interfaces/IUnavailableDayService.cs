using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.ResultModels;

namespace CollectiveMemory.Core.Services.Interfaces
{
    public interface IUnavailableDayService : IBaseService<UnavailableDay, int>
    {
        /// <summary>Marked days from <paramref name="from"/> to <paramref name="to"/>, both inclusive.</summary>
        Task<List<UnavailableDay>> GetRangeAsync(DateOnly from, DateOnly to);

        /// <summary>The next marked days starting at <paramref name="from"/>, soonest first.</summary>
        Task<List<UnavailableDay>> GetUpcomingAsync(DateOnly from, int take);

        /// <summary>Marks a day (or updates its note). Refused when there is a show that day.</summary>
        Task<ResultModel<bool>> SetAsync(DateOnly date, string? note);

        /// <summary>Makes a day free again. Not an error if it was not marked.</summary>
        Task<ResultModel<bool>> ClearAsync(DateOnly date);
    }
}
