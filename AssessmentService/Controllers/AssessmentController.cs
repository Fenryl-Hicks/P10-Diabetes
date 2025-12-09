using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AssessmentService.Services.Interfaces;

namespace AssessmentService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AssessmentController : ControllerBase
    {
        private readonly IAssessmentService _assessmentService;
        private readonly ILogger<AssessmentController> _logger;

        public AssessmentController(IAssessmentService assessmentService, ILogger<AssessmentController> logger)
        {
            _assessmentService = assessmentService;
            _logger = logger;
        }

        /// <summary>
        /// Évalue le niveau de risque de diabète pour un patient
        /// </summary>
        /// <param name="patientId">Identifiant du patient</param>
        /// <returns>Niveau de risque de diabète</returns>
        [HttpGet("{patientId}")]
        public async Task<IActionResult> GetAssessment(int patientId)
        {
            var token = GetToken();
            
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token manquant pour l'évaluation du patient {PatientId}", patientId);
                return Unauthorized("Token manquant");
            }

            _logger.LogInformation("Évaluation du risque pour le patient {PatientId}", patientId);

            var riskLevel = await _assessmentService.AssessPatientRiskAsync(patientId, token);

            if (riskLevel == null)
            {
                _logger.LogWarning("Impossible d'évaluer le patient {PatientId}", patientId);
                return NotFound($"Impossible d'évaluer le patient {patientId}");
            }

            var riskLevelString = riskLevel.ToString();
            _logger.LogInformation("Risque évalué pour le patient {PatientId}: {RiskLevel}", patientId, riskLevelString);

            return Ok(riskLevelString);
        }

        private string? GetToken()
        {
            string? token = null;

            // 1️⃣ PRIORITÉ 1 : Essayer de récupérer depuis le header (appel Backend via Gateway)
            token = Request.Headers["Authorization"].ToString();
            
            if (!string.IsNullOrEmpty(token))
            {
                _logger.LogDebug("Token récupéré depuis le header Authorization");
            }
            else
            {
                // 2️⃣ PRIORITÉ 2 : Si pas de header, chercher dans la session (appel Frontend - normalement ne devrait pas arriver)
                try
                {
                    token = HttpContext.Session.GetString("JwtToken");
                    if (!string.IsNullOrEmpty(token))
                    {
                        _logger.LogDebug("Token récupéré depuis la session");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Erreur lors de la récupération du token depuis la session");
                }
            }

            // 3️⃣ Nettoyer le token (retirer "Bearer " si présent)
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Replace("Bearer ", "").Trim();
            }
            else
            {
                _logger.LogWarning("Aucun token d'authentification trouvé");
            }

            return token;
        }
    }
}
