using P10.Frontend.Models;

namespace P10.Frontend.Services
{
    public interface IPatientApiClient
    {
        Task<List<PatientViewModel>> GetPatientsAsync(string token);
        Task<PatientViewModel?> GetPatientByIdAsync(int id, string token);
        Task<bool> CreatePatientAsync(PatientViewModel model, string token);
        Task<bool> UpdatePatientAsync(PatientViewModel model, string token);
        Task<bool> DeletePatientAsync(int id, string token);
    }
}
