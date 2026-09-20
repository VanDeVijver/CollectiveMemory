using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CollectiveMemory.Core.ResultModels;

namespace CollectiveMemory.Core.Services.Interfaces
{
    public interface IBaseService<TEntity, TKey> where TEntity : class
    {
        // ── CRUD ──────────────────────────────────────────────────────────
        Task<ResultModel<TEntity>> GetByIdAsync(TKey id);
        Task<ResultModel<IEnumerable<TEntity>>> GetAllAsync();
        Task<ResultModel<IEnumerable<TEntity>>> FindAsync(
            Expression<Func<TEntity, bool>> predicate);

        Task<ResultModel<TEntity>> CreateAsync(TEntity entity);
        Task<ResultModel<TEntity>> UpdateAsync(TEntity entity);
        Task<ResultModel<bool>> DeleteAsync(TKey id);

        // ── Utility ───────────────────────────────────────────────────────
        Task<ResultModel<bool>> ExistsAsync(TKey id);
        Task<ResultModel<int>> CountAsync();
        Task<ResultModel<int>> CountAsync(
            Expression<Func<TEntity, bool>> predicate);
    }
}
