namespace P10.Frontend.Services
{
    public interface IAssessmentApiClient
    {
        Task<string?> GetPatientRiskAsync(int patientId, string token);
    }
}
