using System;
namespace ClinicManagementAPI.Core.Abstraction
{
	public interface IEntityResolver
	{
        Task<int> GetRoomIdAsync(int roomNumber);
        Task<int> GetSpecializationIdAsync(string specializationName);
        Task<int> GetRegionIdAsync(int regionNumber);
    }
}

