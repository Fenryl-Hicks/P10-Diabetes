using Moq;
using PatientService.DTOs;
using PatientService.Models;
using PatientService.Repositories.Interfaces;
using PatientService.Services;

namespace PatientServiceTests
{
    public class PatientManagerTests
    {
        private readonly Mock<IPatientRepository> _mockRepository;
        private readonly PatientManager _service;

        public PatientManagerTests()
        {
            _mockRepository = new Mock<IPatientRepository>();
            _service = new PatientManager(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllPatients()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1), GenderId = 1 },
                new Patient { Id = 2, FirstName = "Jane", LastName = "Smith", BirthDate = new DateTime(1985, 5, 15), GenderId = 2 }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(patients);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("John", result[0].FirstName);
            Assert.Equal("Jane", result[1].FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsPatient()
        {
            // Arrange
            var patient = new Patient { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1), GenderId = 1 };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(patient);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Patient?)null);

            // Act
            var result = await _service.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ValidPatient_ReturnsCreatedPatient()
        {
            // Arrange
            var createDto = new CreatePatientDto
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1990, 1, 1),
                GenderId = 1,
                Address = "123 Main St",
                Phone = "1234567890"
            };

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.FirstName);
            Assert.Equal("Doe", result.LastName);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Patient>()), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ExistingPatient_ReturnsUpdatedPatient()
        {
            // Arrange
            var existingPatient = new Patient { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1), GenderId = 1 };
            var updateDto = new UpdatePatientDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                BirthDate = new DateTime(1990, 1, 1),
                GenderId = 2,
                Address = "456 Oak St",
                Phone = "0987654321"
            };

            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingPatient);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(1, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane", result.FirstName);
            Assert.Equal("Smith", result.LastName);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Patient>()), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingPatient_ReturnsNull()
        {
            // Arrange
            var updateDto = new UpdatePatientDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                BirthDate = new DateTime(1990, 1, 1),
                GenderId = 2
            };

            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Patient?)null);

            // Act
            var result = await _service.UpdateAsync(999, updateDto);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ExistingPatient_ReturnsTrue()
        {
            // Arrange
            var patient = new Patient { Id = 1, FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1), GenderId = 1 };
            _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(patient);
            _mockRepository.Setup(r => r.DeleteAsync(It.IsAny<Patient>())).Returns(Task.CompletedTask);
            _mockRepository.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync(1);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Patient>()), Times.Once);
            _mockRepository.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingPatient_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Patient?)null);

            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Patient>()), Times.Never);
        }
    }
}
