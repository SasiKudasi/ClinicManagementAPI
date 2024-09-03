using System;
namespace ClinicManagementAPI.DataAccess.Entities
{
    public class DoctorEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; } // ФИО врача

        public int SpecializationId { get; set; }
        public SpecializationEntity Specialization { get; set; }

        public int? RegionId { get; set; } // Участок для участковых врачей
        public RegionEntity Region { get; set; }

        public int RoomId { get; set; } // Кабинет
        public RoomEntity Room { get; set; }
    }

}

