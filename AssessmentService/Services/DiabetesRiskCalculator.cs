using AssessmentService.Models;
using AssessmentService.Models.DTOs;

namespace AssessmentService.Services
{
    public static class DiabetesRiskCalculator
    {
        private static readonly List<string> Triggers = new()
        {
            "Hémoglobine A1C",
            "Microalbumine",
            "Taille",
            "Poids",
            "Fumeur",
            "Fumeuse",
            "Anormal",
            "Cholestérol",
            "Vertige",
            "Rechute",
            "Réaction",
            "Anticorps"
        };

        /// <summary>
        /// Calcule le niveau de risque de diabète pour un patient
        /// </summary>
        public static RiskLevel CalculateRiskFromPatient(PatientDto patient, List<NoteDto> notes)
        {
            int age = CalculateAge(patient.BirthDate);
            string gender = patient.GenderId == 1 ? "M" : "F";
            var noteContents = notes.Select(n => n.Content).ToList();
            
            return CalculateRisk(age, gender, noteContents);
        }

        /// <summary>
        /// Calcule le niveau de risque de diabète selon l'âge, le genre et les notes
        /// </summary>
        public static RiskLevel CalculateRisk(int age, string gender, List<string> notes)
        {
            int triggerCount = CountTriggers(notes);

            if (triggerCount == 0)
                return RiskLevel.None;

            bool isOver30 = age >= 30;
            bool isMale = gender.Equals("M", StringComparison.OrdinalIgnoreCase);

            // EarlyOnset
            if (!isOver30)
            {
                if (isMale && triggerCount >= 5)
                    return RiskLevel.EarlyOnset;
                if (!isMale && triggerCount >= 7)
                    return RiskLevel.EarlyOnset;
            }
            else
            {
                if (triggerCount >= 8)
                    return RiskLevel.EarlyOnset;
            }

            // InDanger
            if (!isOver30)
            {
                if (isMale && triggerCount >= 3)
                    return RiskLevel.InDanger;
                if (!isMale && triggerCount >= 4)
                    return RiskLevel.InDanger;
            }
            else
            {
                if (triggerCount >= 6)
                    return RiskLevel.InDanger;
            }

            // Borderline
            if (isOver30 && triggerCount >= 2 && triggerCount <= 5)
                return RiskLevel.Borderline;

            return RiskLevel.None;
        }

        /// <summary>
        /// Compte le nombre de termes déclencheurs présents dans les notes (une seule occurrence par trigger)
        /// </summary>
        public static int CountTriggers(List<string> notes)
        {
            string combinedNotes = string.Join(" ", notes).ToLower();
            
            int count = 0;
            foreach (var trigger in Triggers)
            {
                if (combinedNotes.Contains(trigger.ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Calcule l'âge d'un patient à partir de sa date de naissance
        /// </summary>
        public static int CalculateAge(DateTime? birthDate)
        {
            if (!birthDate.HasValue)
                return 0;

            var today = DateTime.Today;
            var age = today.Year - birthDate.Value.Year;
            if (birthDate.Value.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}
