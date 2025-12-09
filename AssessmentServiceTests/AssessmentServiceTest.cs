using AssessmentService.Models;
using AssessmentService.Models.DTOs;
using AssessmentService.Services;

namespace AssessmentServiceTests
{
    public class DiabetesRiskAssessmentTests
    {
        // Tests pour RiskLevel.None - Aucun terme déclencheur
        [Fact]
        public void CalculateRisk_NoTriggers_ReturnsNone()
        {
            int age = 35;
            string gender = "M";
            var notes = new List<string> { "Patient en bonne santé", "Aucun problème détecté" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.None, risk);
        }

        // Tests pour RiskLevel.Borderline
        [Fact]
        public void CalculateRisk_TwoTriggersOver30_ReturnsBorderline()
        {
            int age = 35;
            string gender = "M";
            var notes = new List<string> { "Patient présente Hémoglobine A1C élevée", "Poids excessif détecté" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.Borderline, risk);
        }

        [Fact]
        public void CalculateRisk_FiveTriggersOver30_ReturnsBorderline()
        {
            int age = 40;
            string gender = "F";
            var notes = new List<string> { "Hémoglobine A1C, Microalbumine, Taille, Poids, Fumeur" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.Borderline, risk);
        }

        [Fact]
        public void CalculateRisk_OneTriggerOver30_ReturnsNone()
        {
            int age = 50;
            string gender = "M";
            var notes = new List<string> { "Patient présente Cholestérol élevé" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.None, risk);
        }

        // Tests pour RiskLevel.InDanger - Homme < 30 ans avec 3 déclencheurs
        [Fact]
        public void CalculateRisk_MaleUnder30With3Triggers_ReturnsInDanger()
        {
            int age = 25;
            string gender = "M";
            var notes = new List<string> { "Hémoglobine A1C élevée", "Microalbumine détectée", "Fumeur" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.InDanger, risk);
        }

        [Fact]
        public void CalculateRisk_MaleUnder30With4Triggers_ReturnsInDanger()
        {
            int age = 28;
            string gender = "M";
            var notes = new List<string> { "Hémoglobine A1C, Poids, Taille, Vertiges" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.InDanger, risk);
        }

        // Tests pour RiskLevel.InDanger - Femme < 30 ans avec 4 déclencheurs
        [Fact]
        public void CalculateRisk_FemaleUnder30With4Triggers_ReturnsInDanger()
        {
            int age = 29;
            string gender = "F";
            var notes = new List<string> { "Hémoglobine A1C, Microalbumine, Poids, Anormal" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.InDanger, risk);
        }

        [Fact]
        public void CalculateRisk_FemaleUnder30With3Triggers_ReturnsNone()
        {
            int age = 25;
            string gender = "F";
            var notes = new List<string> { "Hémoglobine A1C, Poids, Fumeur" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.None, risk);
        }

        // Tests pour RiskLevel.InDanger - Plus de 30 ans avec 6-7 déclencheurs
        [Fact]
        public void CalculateRisk_Over30With6Triggers_ReturnsInDanger()
        {
            int age = 35;
            string gender = "M";
            var notes = new List<string> { "Hémoglobine A1C, Microalbumine, Taille, Poids, Fumeur, Cholestérol" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.InDanger, risk);
        }

        [Fact]
        public void CalculateRisk_Over30With7Triggers_ReturnsInDanger()
        {
            int age = 50;
            string gender = "F";
            var notes = new List<string> { "Hémoglobine A1C, Microalbumine, Taille, Poids, Fumeur, Cholestérol, Vertiges" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.InDanger, risk);
        }

        // Tests pour RiskLevel.EarlyOnset - Homme < 30 ans avec 5+ déclencheurs
        [Fact]
        public void CalculateRisk_MaleUnder30With5Triggers_ReturnsEarlyOnset()
        {
            int age = 27;
            string gender = "M";
            var notes = new List<string> { "Hémoglobine A1C, Microalbumine, Taille, Poids, Fumeur" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.EarlyOnset, risk);
        }

        [Fact]
        public void CalculateRisk_MaleUnder30With8Triggers_ReturnsEarlyOnset()
        {
            int age = 20;
            string gender = "M";
            var notes = new List<string> { "Hémoglobine A1C, Microalbumine, Taille, Poids, Fumeur, Cholestérol, Vertiges, Rechute" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.EarlyOnset, risk);
        }

        // Tests pour RiskLevel.EarlyOnset - Femme < 30 ans avec 7+ déclencheurs
        [Fact]
        public void CalculateRisk_FemaleUnder30With7Triggers_ReturnsEarlyOnset()
        {
            int age = 28;
            string gender = "F";
            var notes = new List<string> { "Hémoglobine A1C, Microalbumine, Taille, Poids, Fumeuse, Cholestérol, Vertiges" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.EarlyOnset, risk);
        }

        [Fact]
        public void CalculateRisk_FemaleUnder30With6Triggers_ReturnsInDanger()
        {
            int age = 25;
            string gender = "F";
            var notes = new List<string> { "Hémoglobine A1C, Microalbumine, Taille, Poids, Fumeuse, Anormal" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.InDanger, risk);
        }

        // Tests pour RiskLevel.EarlyOnset - Plus de 30 ans avec 8+ déclencheurs
        [Fact]
        public void CalculateRisk_Over30With8Triggers_ReturnsEarlyOnset()
        {
            int age = 45;
            string gender = "M";
            var notes = new List<string> { "Hémoglobine A1C, Microalbumine, Taille, Poids, Fumeur, Cholestérol, Vertiges, Rechute" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.EarlyOnset, risk);
        }

        [Fact]
        public void CalculateRisk_Over30With11Triggers_ReturnsEarlyOnset()
        {
            int age = 60;
            string gender = "F";
            var notes = new List<string> { "Hémoglobine A1C, Microalbumine, Taille, Poids, Fumeuse, Cholestérol, Vertiges, Rechute, Réaction, Anticorps, Anormal" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.EarlyOnset, risk);
        }

        // Tests edge cases
        [Fact]
        public void CalculateRisk_MaleExactly30With3Triggers_ReturnsBorderline()
        {
            int age = 30;
            string gender = "M";
            var notes = new List<string> { "Hémoglobine A1C, Poids, Fumeur" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.Borderline, risk);
        }

        [Fact]
        public void CalculateRisk_FemaleExactly30With4Triggers_ReturnsBorderline()
        {
            int age = 30;
            string gender = "F";
            var notes = new List<string> { "Hémoglobine A1C, Microalbumine, Poids, Anormal" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.Borderline, risk);
        }

        // Test avec variations de casse et accents
        [Fact]
        public void CalculateRisk_CaseInsensitiveTriggers_CountsCorrectly()
        {
            int age = 35;
            string gender = "M";
            var notes = new List<string> { "hémoglobine a1c élevée", "MICROALBUMINE détectée", "Poids excessif" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.Borderline, risk);
        }

        // Test avec Fumeur et Fumeuse
        [Fact]
        public void CalculateRisk_SmokerVariants_CountsCorrectly()
        {
            int age = 40;
            string gender = "F";
            var notes = new List<string> { "Patient Fumeuse depuis 10 ans", "Hémoglobine A1C élevée" };
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.Borderline, risk);
        }

        // Test notes vides
        [Fact]
        public void CalculateRisk_EmptyNotes_ReturnsNone()
        {
            int age = 40;
            string gender = "M";
            var notes = new List<string>();
            var risk = DiabetesRiskCalculator.CalculateRisk(age, gender, notes);
            Assert.Equal(RiskLevel.None, risk);
        }

        // Tests pour le comptage unique des triggers
        [Fact]
        public void CountTriggers_DuplicateTriggerInSameNote_CountsOnlyOnce()
        {
            var notes = new List<string> { "Patient avec Poids élevé et Poids excessif, contrôle du Poids nécessaire" };
            var count = DiabetesRiskCalculator.CountTriggers(notes);
            Assert.Equal(1, count); // "Poids" apparaît 3 fois mais compte 1 seule fois
        }

        [Fact]
        public void CountTriggers_SameTriggerInDifferentNotes_CountsOnlyOnce()
        {
            var notes = new List<string> 
            { 
                "Patient avec Poids élevé", 
                "Poids excessif détecté",
                "Contrôle du Poids nécessaire" 
            };
            var count = DiabetesRiskCalculator.CountTriggers(notes);
            Assert.Equal(1, count); // "Poids" dans 3 notes différentes compte 1 seule fois
        }

        [Fact]
        public void CountTriggers_MultipleDifferentTriggers_CountsCorrectly()
        {
            var notes = new List<string> { "Hémoglobine A1C élevée, Poids excessif, Taille normale" };
            var count = DiabetesRiskCalculator.CountTriggers(notes);
            Assert.Equal(3, count); // Hémoglobine A1C, Poids, Taille
        }

        // Tests pour CalculateAge
        [Fact]
        public void CalculateAge_ValidBirthDate_ReturnsCorrectAge()
        {
            var birthDate = new DateTime(1990, 1, 1);
            var age = DiabetesRiskCalculator.CalculateAge(birthDate);
            var expectedAge = DateTime.Today.Year - 1990;
            if (new DateTime(DateTime.Today.Year, 1, 1) > DateTime.Today)
                expectedAge--;
            Assert.Equal(expectedAge, age);
        }

        [Fact]
        public void CalculateAge_NullBirthDate_ReturnsZero()
        {
            var age = DiabetesRiskCalculator.CalculateAge(null);
            Assert.Equal(0, age);
        }

        // Tests pour CalculateRiskFromPatient
        [Fact]
        public void CalculateRiskFromPatient_ValidPatientAndNotes_ReturnsCorrectRisk()
        {
            var patient = new PatientDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1990, 1, 1),
                GenderId = 1
            };
            var notes = new List<NoteDto>
            {
                new NoteDto { Id = "1", PatientId = 1, Content = "Hémoglobine A1C élevée" },
                new NoteDto { Id = "2", PatientId = 1, Content = "Poids excessif" }
            };

            var risk = DiabetesRiskCalculator.CalculateRiskFromPatient(patient, notes);
            Assert.Equal(RiskLevel.Borderline, risk);
        }
    }
}