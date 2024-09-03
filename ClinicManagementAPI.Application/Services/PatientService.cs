using ClinicManagementAPI.Application.Contracts;
using ClinicManagementAPI.Core.Abstraction;
using ClinicManagementAPI.DataAccess.Entities;

namespace ClinicManagementAPI.Application.Services
{
    public class PatientService : ICrudService<PatientEntity, PatientDto>
    {
        private readonly ICrudRepository<PatientEntity> _repository;
        private readonly IEntityResolver _entityResolver;

        public PatientService(ICrudRepository<PatientEntity> repository, IEntityResolver resolver)
        {
            _repository = repository;
            _entityResolver = resolver;
        }

        public async Task<PatientDto> GetById(int id)
        {
            var patient = await _repository.GetWithRelatedDataAsync(
           d => d.Id == id,
           d => d.Region);
            var pat = patient.FirstOrDefault();
            return MapToDto(pat);

        }

        public async Task<IEnumerable<PatientDto>> GetList(
                                                string sortBy = "LastName",
                                                int pageNumber = 1,
                                                int pageSize = 10,
                                                bool desc = false)
        {
            var patients = await _repository.GetWithRelatedDataAsync(
                           null,
                           d => d.Region);

            var sortedPatients = sortBy switch
            {
                "LastName" => desc ? patients.OrderByDescending(p => p.LastName) : patients.OrderBy(p => p.LastName),
                "FirstName" => desc ? patients.OrderByDescending(p => p.FirstName) : patients.OrderBy(p => p.FirstName),
                "MiddleName" => desc ? patients.OrderByDescending(p => p.MiddleName) : patients.OrderBy(p => p.MiddleName),
                "BirthDate" => desc ? patients.OrderByDescending(p => p.BirthDate) : patients.OrderBy(p => p.BirthDate),
                "Gender" => desc ? patients.OrderByDescending(p => p.Gender) : patients.OrderBy(p => p.Gender),
                "RegionName" => desc ? patients.OrderByDescending(p => p.Region.Number) : patients.OrderBy(p => p.Region.Number),
                _ => desc ? patients.OrderByDescending(p => p.LastName) : patients.OrderBy(p => p.LastName)
            };

            var pagedPatients = sortedPatients
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedPatients.Select(p => MapToDto(p)).ToList();
        }


        public async Task<PatientEntity> UpdateAsync(PatientDto entity)
        {
            var patient = await _repository.GetByIdAsync(entity.Id);
            if (patient == null)
                return null;
            var pat = await MapDtoToEntity(entity);
            return await _repository.PutAsync(pat);
        }

        public async Task<PatientDto> CreateAsync(PatientDto dto)
        {
            var patient = await MapDtoToEntity(dto);
            await _repository.PostAsync(patient);
            return MapToDto(patient);

        }

        public async Task DeleteAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            if (patient != null) await _repository.DeleteAsync(id);
        }

        private PatientDto MapToDto(PatientEntity entity)
        {
            if (entity == null) return null;

            return new PatientDto
            (
                entity.Id,
                entity.LastName,
                entity.FirstName,
                entity.MiddleName,
                entity.Address,
                entity.BirthDate,
                entity.Gender,
                entity.Region.Number
            );
        }

        private async Task<PatientEntity> MapDtoToEntity(PatientDto dto)
        {
            if (dto == null) return null;
            return new PatientEntity
            {
                Id = dto.Id,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                Address = dto.Address,
                BirthDate = dto.BirthDay,
                Gender = dto.Gender,
                RegionId = await _entityResolver.GetRegionIdAsync(dto.RegionNamber) // Получение идентификатора из DTO
            };
        }
    }
}

