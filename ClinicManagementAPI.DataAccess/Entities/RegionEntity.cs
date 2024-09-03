using System;
namespace ClinicManagementAPI.DataAccess.Entities
{
    public class RegionEntity
    {
        public int Id { get; set; }
        public int Number { get; set; } // Номер участка

        // Связи
        public ICollection<PatientEntity> Patients { get; set; } = new List<PatientEntity>();
        public ICollection<DoctorEntity> Doctors { get; set; } = new List<DoctorEntity>();
    }

}

