using System;
namespace ClinicManagementAPI.Application.Contracts
{
	public record PatientDto(
		int Id,
		string LastName,
		string FirstName,
		string MiddleName,
		string Address,
		DateTime BirthDay,
		char Gender,
		int RegionNamber);
}

