using PatientService.DTOs;

namespace PatientService.Services.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto?> GetByIdAsync(Guid id);
        Task<PatientDto> CreateAsync(PatientDto dto);

        Task<PatientDto?> UpdateAsync(Guid id, PatientDto dto);
        Task<bool> DeleteAsync(Guid id);

    }
}
