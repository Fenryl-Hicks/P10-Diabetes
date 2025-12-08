using AssessmentService.Models.DTOs;

namespace AssessmentService.Services.Interfaces
{
    public interface IPatientApiService
    {
        Task<PatientDto?> GetPatientByIdAsync(int patientId, string token);
    }
}
