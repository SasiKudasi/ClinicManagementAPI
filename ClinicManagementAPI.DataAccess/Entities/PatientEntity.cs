using System;
namespace ClinicManagementAPI.DataAccess.Entities
{
    public class PatientEntity
    {
        public int Id { get; set; }
        public string LastName { get; set; } // Фамилия
        public string FirstName { get; set; } // Имя
        public string MiddleName { get; set; } // Отчество
        public string Address { get; set; } // Адрес
        public DateTime BirthDate { get; set; } // Дата рождения
        public char Gender { get; set; } // Пол

        public int RegionId { get; set; } // Участок
        public RegionEntity Region { get; set; }
    }

}

