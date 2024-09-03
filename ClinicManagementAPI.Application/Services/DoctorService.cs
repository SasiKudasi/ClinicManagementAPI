using ClinicManagementAPI.Application.Contracts;
using ClinicManagementAPI.Core.Abstraction;
using ClinicManagementAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace ClinicManagementAPI.Application.Services
{
    public class DoctorService : ICrudService<DoctorEntity, DoctorDto>
    {
        private readonly ICrudRepository<DoctorEntity> _repository;
        private readonly IEntityResolver _entityResolver;

        public DoctorService(ICrudRepository<DoctorEntity> repository, IEntityResolver resolver)
        {
            _repository = repository;
            _entityResolver = resolver;
        }

        public async Task<DoctorDto> GetById(int id)
        {

            var doctor = await _repository.GetWithRelatedDataAsync(
            d => d.Id == id,
            d => d.Specialization,
            d => d.Room,
            d => d.Region);
            var doc = doctor.FirstOrDefault();
            return MapToDto(doc);
        }

        public async Task<IEnumerable<DoctorDto>> GetList(
            string sortBy = "FullName",
            int pageNumber = 1,
            int pageSize = 10,
            bool desc = false)
        {

            var doctors = await _repository.GetWithRelatedDataAsync(
                           null,
                           d => d.Specialization,
                           d => d.Room,
                           d => d.Region);

            var sortedDoctors = sortBy switch
            {
                "FullName" => desc ? doctors.OrderByDescending(d => d.FullName) : doctors.OrderBy(d => d.FullName),
                "Specialization" => desc ? doctors.OrderByDescending(d => d.Specialization.Name) : doctors.OrderBy(d => d.Specialization.Name),
                "RoomNumber" => desc ? doctors.OrderByDescending(d => d.Room.Number) : doctors.OrderBy(d => d.Room.Number),
                "RegionNumber" => desc ? doctors.OrderByDescending(d => d.Region.Number) : doctors.OrderBy(d => d.Region.Number),
                _ => desc ? doctors.OrderByDescending(d => d.FullName) : doctors.OrderBy(d => d.FullName)
            };

            var pagedDoctors = sortedDoctors
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedDoctors.Select(d => MapToDto(d)).ToList();
        }

        public async Task<DoctorEntity> UpdateAsync(DoctorDto entity)
        {
            var doctor = await _repository.GetByIdAsync(entity.Id);
            if (doctor == null)
                return null;
            var doc = await MapDtoToEntity(entity);
            return await _repository.PutAsync(doc);
        }

        public async Task<DoctorDto> CreateAsync(DoctorDto dto)
        {
            var doc = await MapDtoToEntity(dto);
            await _repository.PostAsync(doc);
            return MapToDto(doc);

        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);
            if (doctor != null) await _repository.DeleteAsync(id);
        }

        private DoctorDto MapToDto(DoctorEntity entity)
        {
            if (entity == null) return null;


            return new DoctorDto
            (
                 entity.Id,
                 entity.FullName,
                 entity.Room.Number,
                 entity.Specialization.Name,
                 entity.Region.Number

            );
        }

        private async Task<DoctorEntity> MapDtoToEntity(DoctorDto dto)
        {
            if (dto == null) return null;
            var entity = new DoctorEntity
            {
                Id = dto.Id,
                FullName = dto.FullName,
                RegionId = await _entityResolver.GetRegionIdAsync(dto.RegionNumber),
                RoomId = await _entityResolver.GetRoomIdAsync(dto.RoomNumber),
                SpecializationId = await _entityResolver.GetSpecializationIdAsync(dto.SpecializationName)
            };
            return entity;
        }
    }
}

