using System;
using ClinicManagementAPI.Core.Abstraction;
using ClinicManagementAPI.DataAccess.Entities;

namespace ClinicManagementAPI.Application.Utilities
{
	public class EntityResolver : IEntityResolver
    {
        private readonly ICrudRepository<RoomEntity> _roomRepository;
        private readonly ICrudRepository<SpecializationEntity> _specializationRepository;
        private readonly ICrudRepository<RegionEntity> _regionRepository;

        public EntityResolver(
            ICrudRepository<RoomEntity> roomRepository,
            ICrudRepository<SpecializationEntity> specializationRepository,
            ICrudRepository<RegionEntity> regionRepository)
        {
            _roomRepository = roomRepository;
            _specializationRepository = specializationRepository;
            _regionRepository = regionRepository;
        }

        public async Task<int> GetRoomIdAsync(int roomNumber)
        {
            var roomList = await _roomRepository.GetAsync();
            var room = roomList.FirstOrDefault(r => r.Number == roomNumber);
            return room?.Id ?? throw new KeyNotFoundException("Room not found");
        }

        public async Task<int> GetSpecializationIdAsync(string specializationName)
        {
            var specializations = await _specializationRepository.GetAsync();
            var specialization = specializations.FirstOrDefault(s => s.Name == specializationName);
            return specialization?.Id ?? throw new KeyNotFoundException("Specialization not found");
        }

        public async Task<int> GetRegionIdAsync(int regionNumber)
        {
            var regions = await _regionRepository.GetAsync();
            var region = regions.FirstOrDefault(r => r.Number == regionNumber);
            return region.Id;
        }
    }
}

