using System;
namespace ClinicManagementAPI.DataAccess.Entities
{
    public class RoomEntity
    {
        public int Id { get; set; }
        public int Number { get; set; } // Номер кабинета

        // Связь с врачами
        public ICollection<DoctorEntity> Doctors { get; set; } = new List<DoctorEntity>();
    }

}

