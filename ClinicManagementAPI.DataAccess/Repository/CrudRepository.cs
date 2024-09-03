using ClinicManagementAPI.Core.Abstraction;
using Microsoft.EntityFrameworkCore;

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
              await  _context.SaveChangesAsync();
            }
        }

        public async Task<List<T>> GetAsync()
        {
            var entitys = await _dbSet.ToListAsync();
            return entitys;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity =  await _dbSet.FindAsync(id);
            return entity;
        }

        public async Task<T> PostAsync(T obj)
        {
            await _dbSet.AddAsync(obj);
            await _context.SaveChangesAsync();
            return obj;
        }

        public async Task<T> PutAsync(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified; 
            await _context.SaveChangesAsync();
            return obj;
        }
    }
}

