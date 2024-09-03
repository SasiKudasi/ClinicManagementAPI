using System;
namespace ClinicManagementAPI.DataAccess.Entities
{
    public class SpecializationEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } // Название специализации

        // Связь с врачами
        public ICollection<DoctorEntity> Doctors { get; set; } = new List<DoctorEntity>();
    }

}

