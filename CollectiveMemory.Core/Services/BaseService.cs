using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CollectiveMemory.Core.Data;
using CollectiveMemory.Core.Entities;
using CollectiveMemory.Core.ResultModels;
using CollectiveMemory.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CollectiveMemory.Core.Services
{
    public abstract class BaseService<TEntity, TKey> : IBaseService<TEntity, TKey> where TEntity : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly ILogger<BaseService<TEntity, TKey>> _logger;

        protected BaseService(ApplicationDbContext context,
            ILogger<BaseService<TEntity, TKey>> logger)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _logger = logger;
        }
        public virtual async Task<ResultModel<int>> CountAsync()
        {
            try
            {
                var count = await _dbSet.CountAsync();
                return ResultModel<int>.Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het tellen van {Entity}", typeof(TEntity).Name);
                return ResultModel<int>.Fail($"Onverwachte fout: {ex.Message}");
            }
        }

        public virtual async Task<ResultModel<int>> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var count = await _dbSet.CountAsync(predicate);
                return ResultModel<int>.Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het tellen van {Entity} met predicate", typeof(TEntity).Name);
                return ResultModel<int>.Fail($"Onverwachte fout: {ex.Message}");
            }
        }

        public virtual async Task<ResultModel<TEntity>> CreateAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("{Entity} aangemaakt met id {Id}",
                    typeof(TEntity).Name, entity.Id);
                return ResultModel<TEntity>.Ok(entity);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database fout bij verwijderen van {Entity} id {Id}",
                   typeof(TEntity).Name);
                return ResultModel<TEntity>.Fail($"Database fout: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onverwachte fout bij aanmaken van {Entity}",
                    typeof(TEntity).Name);
                return ResultModel<TEntity>.Fail($"Onverwachte fout: {ex.Message}");
            }
        }

        public virtual async Task<ResultModel<bool>> DeleteAsync(TKey id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity is null)
                    return ResultModel<bool>.Fail($"{typeof(TEntity).Name} met id '{id}' werd niet gevonden.");

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("{Entity} verwijderd met id {Id}",
                    typeof(TEntity).Name, id);

                return ResultModel<bool>.Ok(true);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database fout bij verwijderen van {Entity} id {Id}",
                    typeof(TEntity).Name, id);
                return ResultModel<bool>.Fail($"Database fout: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onverwachte fout bij verwijderen van {Entity} id {Id}",
                    typeof(TEntity).Name, id);
                return ResultModel<bool>.Fail($"Onverwachte fout: {ex.Message}");
            }
        }

        public virtual async Task<ResultModel<bool>> ExistsAsync(TKey id)
        {
            try
            {
                var exists = await _dbSet.AnyAsync(e => e.Id.Equals(id));
                return ResultModel<bool>.Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het checken of de {Entity} bestaat", typeof(TEntity).Name);
                return ResultModel<bool>.Fail($"Onverwachte fout: {ex.Message}");
            }
        }

        public virtual async Task<ResultModel<IEnumerable<TEntity>>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entities = await _dbSet.AsNoTracking()
                    .Where(predicate)
                    .ToListAsync();
                return ResultModel<IEnumerable<TEntity>>.Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het ophalen van alle {Entity}", typeof(TEntity).Name);
                return ResultModel<IEnumerable<TEntity>>.Fail($"Onverwachte fout: {ex.Message}");
            }
        }

        public virtual async Task<ResultModel<IEnumerable<TEntity>>> GetAllAsync()
        {
            try
            {
                var entities = await _dbSet.AsNoTracking().ToListAsync();
                return ResultModel<IEnumerable<TEntity>>.Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij het ophalen van alle {Entity}", typeof(TEntity).Name);
                return ResultModel<IEnumerable<TEntity>>.Fail($"Onverwachte fout: {ex.Message}");
            }
        }

        public virtual async Task<ResultModel<TEntity>> GetByIdAsync(TKey id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);

                return entity is null ? ResultModel<TEntity>.Fail($"{typeof(TEntity).Name} met id {id} werd niet gevonden.")
                    : ResultModel<TEntity>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij ophalen van {Entity} met id {Id}", typeof(TEntity).Name, id);
                return ResultModel<TEntity>.Fail($"Onverwachte fout: {ex.Message}");
            }
        }

        public virtual async Task<ResultModel<TEntity>> UpdateAsync(TEntity entity)
        {
            try
            {
                var exists = await _dbSet.AnyAsync(e => e.Id == entity.Id);
                if (!exists)
                    ResultModel<TEntity>.Fail($"{typeof(TEntity).Name} met id {entity.Id} bestaat niet.");

                _dbSet.Update(entity);
                await _context.SaveChangesAsync();

                _logger.LogInformation("{Entity} bijgewerkt met id {Id}",
                    typeof(TEntity).Name, entity.Id);

                return ResultModel<TEntity>.Ok(entity);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency fout bij bijwerken van {Entity} id {Id}",
                    typeof(TEntity).Name, entity.Id);
                return ResultModel<TEntity>.Fail("Het record werd gewijzigd door een andere gebruiker. Herlaad en probeer opnieuw.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database fout bij bijwerken van {Entity}",
                    typeof(TEntity).Name);
                return ResultModel<TEntity>.Fail($"Database fout: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Onverwachte fout bij bijwerken van {Entity}",
                    typeof(TEntity).Name);
                return ResultModel<TEntity>.Fail($"Onverwachte fout: {ex.Message}");
            }
        }

        // ── PROTECTED HELPERS (available to derived services) ─────────────

        /// <summary>
        /// Returns a queryable with no tracking — use this in derived services
        /// when you need to chain .Include() or extra .Where() before executing.
        /// </summary>
        protected IQueryable<TEntity> Query() => _dbSet.AsNoTracking();

        /// <summary>
        /// Same as Query() but WITH change tracking — use when you intend to
        /// update the returned entity immediately.
        /// </summary>
        protected IQueryable<TEntity> TrackingQuery() => _dbSet.AsTracking();
    }
}
