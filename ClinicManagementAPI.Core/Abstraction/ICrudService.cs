using System;
namespace ClinicManagementAPI.Core.Abstraction
{
	public interface ICrudService<TEntity, TDto>
		where TEntity : class
		where TDto : class
	{
		public Task<TDto> GetById(int id);
        public Task<IEnumerable<TDto>> GetList(
                                                string sortBy = "LastName",
                                                int pageNumber = 1,
                                                int pageSize = 10,
                                                bool desc = false);
        public Task<TEntity> UpdateAsync(TDto entity);
        public Task<TDto> CreateAsync(TDto dto);
        public Task DeleteAsync(int id);

    }
}

