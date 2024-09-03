using System;
namespace ClinicManagementAPI.Application.Contracts
{
	public record DoctorDto (
		int Id,
		string FullName,
		int RoomNumber,
		string SpecializationName,
		int RegionNumber);
}

