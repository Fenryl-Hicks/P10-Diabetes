using PatientService.Models;
using PatientService.DTOs;

namespace PatientService.Utilities
{
    public static class Mapping
    {
        public static PatientDto ToPatientDto(Patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                BirthDate = patient.BirthDate,
                GenderId = patient.GenderId,
                GenderName = patient.Gender?.Name ?? string.Empty,
                Address = patient.Address,
                Phone = patient.Phone
            };
        }

        public static Patient ToPatient(CreatePatientDto dto)
        {
            return new Patient
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                GenderId = dto.GenderId,
                Address = dto.Address,
                Phone = dto.Phone
            };
        }

        public static void UpdatePatientFromDto(Patient patient, UpdatePatientDto dto)
        {
            patient.FirstName = dto.FirstName;
            patient.LastName = dto.LastName;
            patient.BirthDate = dto.BirthDate;
            patient.GenderId = dto.GenderId;
            patient.Address = dto.Address;
            patient.Phone = dto.Phone;
        }
    }
}
