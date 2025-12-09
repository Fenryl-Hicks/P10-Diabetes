using AssessmentService.Models;

namespace AssessmentService.Services.Interfaces
{
    public interface IAssessmentService
    {
        Task<RiskLevel?> AssessPatientRiskAsync(int patientId, string token);
    }
}
