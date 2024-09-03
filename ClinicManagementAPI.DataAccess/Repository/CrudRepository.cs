using ClinicManagementAPI.Core.Abstraction;
using ClinicManagementAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ClinicManagementAPI.DataAccess.Repository
{
    public class CrudRepository<T> : ICrudRepository<T> where T : class
    {
        private readonly ClinicManagementDbContext _context;
        private DbSet<T> _dbSet;

        public CrudRepository(ClinicManagementDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<T>> GetAsync()
        {
            var entitys = await _dbSet.ToListAsync();
            return entitys;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> PostAsync(T obj)
        {
            var keyProperty = typeof(T).GetProperty("Id");
            if (keyProperty != null)
            {
                keyProperty.SetValue(obj, default(int)); // Или default(TId), в зависимости от типа идентификатора
            }

            _dbSet.Add(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<T> PutAsync(T obj)
        {
            var keyProperty = typeof(T).GetProperty("Id");
            if (keyProperty != null)
            {
                var id = (int)keyProperty.GetValue(obj);
                var existingEntity = await _dbSet.FindAsync(id);

                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).CurrentValues.SetValues(obj);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException("Entity not found.");
                }
            }
            
            return obj;
        }

        public async Task<IEnumerable<T>> GetWithRelatedDataAsync(
         Expression<Func<T, bool>> predicate = null,
         params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync();
        }


    }
}

