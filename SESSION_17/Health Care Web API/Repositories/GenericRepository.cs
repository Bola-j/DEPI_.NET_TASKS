using Health_Care_Web_API.Data;
using Health_Care_Web_API.Models;
using Health_Care_Web_API.Results;
using Microsoft.EntityFrameworkCore;

namespace Health_Care_Web_API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly HEALTH_CARE_SYSTEM_DBContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(HEALTH_CARE_SYSTEM_DBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Query(bool asNoTracking = true)
        {
            var query = _dbSet.AsQueryable();
            return asNoTracking ? query.AsNoTracking() : query;
        }

        public async Task<GenericResult<PagedResult<T>>> GetAllAsync(string? search, int pageNumber, int pageSize)
        {
            try
            {
                IQueryable<T> query = Query();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    var searchValue = search.Trim();
                    query = query.Where(e => EF.Property<string>(e, "Name") != null &&
                                             EF.Functions.Like(EF.Property<string>(e, "Name"), $"%{searchValue}%"));
                }

                var totalCount = await query.CountAsync();
                var data = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return GenericResult<PagedResult<T>>.Success(new PagedResult<T>(data, pageNumber, pageSize, totalCount));
            }
            catch (Exception ex)
            {
                return GenericResult<PagedResult<T>>.Failure(ex.Message);
            }
        }

        public async Task<GenericResult<T>> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            return entity is null
                ? GenericResult<T>.Failure($"{typeof(T).Name} with id {id} was not found.")
                : GenericResult<T>.Success(entity);
        }

        public async Task<GenericResult<T>> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return GenericResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return GenericResult<T>.Failure(ex.Message);
            }
        }

        public async Task<GenericResult<T>> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return GenericResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return GenericResult<T>.Failure(ex.Message);
            }
        }

        public async Task<GenericResult<T>> DeleteAsync(int id)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (entity is null)
            {
                return GenericResult<T>.Failure($"{typeof(T).Name} with id {id} was not found.");
            }

            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return GenericResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return GenericResult<T>.Failure(ex.Message);
            }
        }
    }
}
