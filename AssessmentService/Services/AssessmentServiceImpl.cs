using AssessmentService.Models;
using AssessmentService.Services.Interfaces;

namespace AssessmentService.Services
{
    public class AssessmentServiceImpl : IAssessmentService
    {
        private readonly IPatientApiService _patientApiService;
        private readonly INoteApiService _noteApiService;
        private readonly ILogger<AssessmentServiceImpl> _logger;

        public AssessmentServiceImpl(
            IPatientApiService patientApiService,
            INoteApiService noteApiService,
            ILogger<AssessmentServiceImpl> logger)
        {
            _patientApiService = patientApiService;
            _noteApiService = noteApiService;
            _logger = logger;
        }

        public async Task<RiskLevel?> AssessPatientRiskAsync(int patientId, string token)
        {
            try
            {
                var patient = await _patientApiService.GetPatientByIdAsync(patientId, token);
                if (patient == null)
                {
                    _logger.LogWarning("Patient {PatientId} not found", patientId);
                    return null;
                }

                var notes = await _noteApiService.GetNotesByPatientIdAsync(patientId, token);
                
                var riskLevel = DiabetesRiskCalculator.CalculateRiskFromPatient(patient, notes);

                return riskLevel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assessing patient {PatientId}", patientId);
                return null;
            }
        }
    }
}
