using System;
namespace ClinicManagementAPI.Core.Abstraction
{
	public interface ICrudRepository<T> where T: class
	{
        public Task DeleteAsync(int id);
        public Task<List<T>> GetAsync();
        public Task<T> GetByIdAsync(int id);
        public Task<T> PostAsync(T obj);
        public Task<T> PutAsync(T obj);
    }
}

