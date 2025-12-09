using Moq;
using Microsoft.Extensions.Logging;
using AssessmentService.Models;
using AssessmentService.Models.DTOs;
using AssessmentService.Services;
using AssessmentService.Services.Interfaces;

namespace AssessmentServiceTests
{
    public class AssessmentServiceImplTests
    {
        private readonly Mock<IPatientApiService> _mockPatientApiService;
        private readonly Mock<INoteApiService> _mockNoteApiService;
        private readonly Mock<ILogger<AssessmentServiceImpl>> _mockLogger;
        private readonly AssessmentServiceImpl _service;

        public AssessmentServiceImplTests()
        {
            _mockPatientApiService = new Mock<IPatientApiService>();
            _mockNoteApiService = new Mock<INoteApiService>();
            _mockLogger = new Mock<ILogger<AssessmentServiceImpl>>();
            _service = new AssessmentServiceImpl(
                _mockPatientApiService.Object,
                _mockNoteApiService.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task AssessPatientRiskAsync_ValidPatientWithNoTriggers_ReturnsNone()
        {
            // Arrange
            var patientId = 1;
            var token = "fake-token";

            var patient = new PatientDto
            {
                Id = patientId,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1990, 1, 1),
                GenderId = 1
            };

            var notes = new List<NoteDto>
            {
                new NoteDto { Id = "1", PatientId = patientId, Content = "Patient en bonne santé" },
                new NoteDto { Id = "2", PatientId = patientId, Content = "Aucun problème détecté" }
            };

            _mockPatientApiService.Setup(s => s.GetPatientByIdAsync(patientId, token))
                .ReturnsAsync(patient);
            _mockNoteApiService.Setup(s => s.GetNotesByPatientIdAsync(patientId, token))
                .ReturnsAsync(notes);

            // Act
            var result = await _service.AssessPatientRiskAsync(patientId, token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(RiskLevel.None, result);
            _mockPatientApiService.Verify(s => s.GetPatientByIdAsync(patientId, token), Times.Once);
            _mockNoteApiService.Verify(s => s.GetNotesByPatientIdAsync(patientId, token), Times.Once);
        }

        [Fact]
        public async Task AssessPatientRiskAsync_PatientNotFound_ReturnsNull()
        {
            // Arrange
            var patientId = 999;
            var token = "fake-token";

            _mockPatientApiService.Setup(s => s.GetPatientByIdAsync(patientId, token))
                .ReturnsAsync((PatientDto?)null);

            // Act
            var result = await _service.AssessPatientRiskAsync(patientId, token);

            // Assert
            Assert.Null(result);
            _mockPatientApiService.Verify(s => s.GetPatientByIdAsync(patientId, token), Times.Once);
            _mockNoteApiService.Verify(s => s.GetNotesByPatientIdAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AssessPatientRiskAsync_MaleUnder30With5Triggers_ReturnsEarlyOnset()
        {
            // Arrange
            var patientId = 2;
            var token = "fake-token";

            var patient = new PatientDto
            {
                Id = patientId,
                FirstName = "Young",
                LastName = "Male",
                BirthDate = DateTime.Today.AddYears(-25),
                GenderId = 1 // Male
            };

            var notes = new List<NoteDto>
            {
                new NoteDto { Id = "1", PatientId = patientId, Content = "Hémoglobine A1C élevée" },
                new NoteDto { Id = "2", PatientId = patientId, Content = "Microalbumine détectée" },
                new NoteDto { Id = "3", PatientId = patientId, Content = "Taille et Poids anormaux" },
                new NoteDto { Id = "4", PatientId = patientId, Content = "Patient fumeur" }
            };

            _mockPatientApiService.Setup(s => s.GetPatientByIdAsync(patientId, token))
                .ReturnsAsync(patient);
            _mockNoteApiService.Setup(s => s.GetNotesByPatientIdAsync(patientId, token))
                .ReturnsAsync(notes);

            // Act
            var result = await _service.AssessPatientRiskAsync(patientId, token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(RiskLevel.EarlyOnset, result);
        }

        [Fact]
        public async Task AssessPatientRiskAsync_FemaleOver30With7Triggers_ReturnsInDanger()
        {
            // Arrange
            var patientId = 3;
            var token = "fake-token";

            var patient = new PatientDto
            {
                Id = patientId,
                FirstName = "Adult",
                LastName = "Female",
                BirthDate = DateTime.Today.AddYears(-40),
                GenderId = 2 // Female
            };

            var notes = new List<NoteDto>
            {
                new NoteDto { Id = "1", PatientId = patientId, Content = "Hémoglobine A1C, Microalbumine" },
                new NoteDto { Id = "2", PatientId = patientId, Content = "Taille, Poids, Cholestérol" },
                new NoteDto { Id = "3", PatientId = patientId, Content = "Fumeuse, Vertiges" }
            };

            _mockPatientApiService.Setup(s => s.GetPatientByIdAsync(patientId, token))
                .ReturnsAsync(patient);
            _mockNoteApiService.Setup(s => s.GetNotesByPatientIdAsync(patientId, token))
                .ReturnsAsync(notes);

            // Act
            var result = await _service.AssessPatientRiskAsync(patientId, token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(RiskLevel.InDanger, result);
        }

        [Fact]
        public async Task AssessPatientRiskAsync_Over30With3Triggers_ReturnsBorderline()
        {
            // Arrange
            var patientId = 4;
            var token = "fake-token";

            var patient = new PatientDto
            {
                Id = patientId,
                FirstName = "Borderline",
                LastName = "Patient",
                BirthDate = DateTime.Today.AddYears(-35),
                GenderId = 1
            };

            var notes = new List<NoteDto>
            {
                new NoteDto { Id = "1", PatientId = patientId, Content = "Hémoglobine A1C élevée" },
                new NoteDto { Id = "2", PatientId = patientId, Content = "Poids anormal" },
                new NoteDto { Id = "3", PatientId = patientId, Content = "Fumeur" }
            };

            _mockPatientApiService.Setup(s => s.GetPatientByIdAsync(patientId, token))
                .ReturnsAsync(patient);
            _mockNoteApiService.Setup(s => s.GetNotesByPatientIdAsync(patientId, token))
                .ReturnsAsync(notes);

            // Act
            var result = await _service.AssessPatientRiskAsync(patientId, token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(RiskLevel.Borderline, result);
        }

        [Fact]
        public async Task AssessPatientRiskAsync_EmptyNotes_ReturnsNone()
        {
            // Arrange
            var patientId = 5;
            var token = "fake-token";

            var patient = new PatientDto
            {
                Id = patientId,
                FirstName = "Healthy",
                LastName = "Person",
                BirthDate = DateTime.Today.AddYears(-30),
                GenderId = 2
            };

            var notes = new List<NoteDto>();

            _mockPatientApiService.Setup(s => s.GetPatientByIdAsync(patientId, token))
                .ReturnsAsync(patient);
            _mockNoteApiService.Setup(s => s.GetNotesByPatientIdAsync(patientId, token))
                .ReturnsAsync(notes);

            // Act
            var result = await _service.AssessPatientRiskAsync(patientId, token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(RiskLevel.None, result);
        }

        [Fact]
        public async Task AssessPatientRiskAsync_ExceptionThrown_ReturnsNull()
        {
            // Arrange
            var patientId = 6;
            var token = "fake-token";

            _mockPatientApiService.Setup(s => s.GetPatientByIdAsync(patientId, token))
                .ThrowsAsync(new Exception("API Error"));

            // Act
            var result = await _service.AssessPatientRiskAsync(patientId, token);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AssessPatientRiskAsync_Over30With8Triggers_ReturnsEarlyOnset()
        {
            // Arrange
            var patientId = 7;
            var token = "fake-token";

            var patient = new PatientDto
            {
                Id = patientId,
                FirstName = "High",
                LastName = "Risk",
                BirthDate = DateTime.Today.AddYears(-50),
                GenderId = 1
            };

            var notes = new List<NoteDto>
            {
                new NoteDto { Id = "1", PatientId = patientId, Content = "Hémoglobine A1C, Microalbumine, Taille, Poids" },
                new NoteDto { Id = "2", PatientId = patientId, Content = "Fumeur, Cholestérol, Vertiges, Rechute" }
            };

            _mockPatientApiService.Setup(s => s.GetPatientByIdAsync(patientId, token))
                .ReturnsAsync(patient);
            _mockNoteApiService.Setup(s => s.GetNotesByPatientIdAsync(patientId, token))
                .ReturnsAsync(notes);

            // Act
            var result = await _service.AssessPatientRiskAsync(patientId, token);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(RiskLevel.EarlyOnset, result);
        }
    }
}
