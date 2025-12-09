using Moq;
using NoteService.Models.DTOs;
using NoteService.Models.Entities;
using NoteService.Repositories.Interfaces;
using NoteService.Services;

namespace NoteServiceTests
{
    public class NoteServicesTests
    {
        private readonly Mock<INoteRepository> _mockRepository;
        private readonly NoteServices _service;

        public NoteServicesTests()
        {
            _mockRepository = new Mock<INoteRepository>();
            _service = new NoteServices(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllNotes()
        {
            // Arrange
            var notes = new List<Note>
            {
                new Note { Id = "1", PatientId = 1, Content = "Note 1", IsDeleted = false },
                new Note { Id = "2", PatientId = 2, Content = "Note 2", IsDeleted = false }
            };
            _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(notes);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Note 1", result[0].Content);
            Assert.Equal("Note 2", result[1].Content);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsNote()
        {
            // Arrange
            var note = new Note { Id = "1", PatientId = 1, Content = "Test Note", IsDeleted = false };
            _mockRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(note);

            // Act
            var result = await _service.GetByIdAsync("1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal("Test Note", result.Content);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync("999")).ReturnsAsync((Note?)null);

            // Act
            var result = await _service.GetByIdAsync("999");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByPatientIdAsync_ReturnsNotesForPatient()
        {
            // Arrange
            var notes = new List<Note>
            {
                new Note { Id = "1", PatientId = 1, Content = "Note 1", IsDeleted = false },
                new Note { Id = "2", PatientId = 1, Content = "Note 2", IsDeleted = false }
            };
            _mockRepository.Setup(r => r.GetByPatientIdAsync(1)).ReturnsAsync(notes);

            // Act
            var result = await _service.GetByPatientIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, n => Assert.Equal(1, n.PatientId));
        }

        [Fact]
        public async Task CreateAsync_ValidNote_ReturnsCreatedNote()
        {
            // Arrange
            var createDto = new CreateNoteDto
            {
                PatientId = 1,
                Content = "New Note"
            };

            _mockRepository.Setup(r => r.AddAsync(It.IsAny<Note>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.PatientId);
            Assert.Equal("New Note", result.Content);
            _mockRepository.Verify(r => r.AddAsync(It.IsAny<Note>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ExistingNote_ReturnsUpdatedNote()
        {
            // Arrange
            var existingNote = new Note { Id = "1", PatientId = 1, Content = "Old Content", IsDeleted = false };
            var updateDto = new UpdateNoteDto
            {
                Content = "Updated Content"
            };

            _mockRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(existingNote);
            _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Note>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync("1", updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Content", result.Content);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Note>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingNote_ReturnsNull()
        {
            // Arrange
            var updateDto = new UpdateNoteDto
            {
                Content = "Updated Content"
            };

            _mockRepository.Setup(r => r.GetByIdAsync("999")).ReturnsAsync((Note?)null);

            // Act
            var result = await _service.UpdateAsync("999", updateDto);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Note>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ExistingNote_ReturnsTrue()
        {
            // Arrange
            var note = new Note { Id = "1", PatientId = 1, Content = "Note to delete", IsDeleted = false };
            _mockRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(note);
            _mockRepository.Setup(r => r.DeleteAsync(It.IsAny<Note>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteAsync("1");

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Note>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingNote_ReturnsFalse()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetByIdAsync("999")).ReturnsAsync((Note?)null);

            // Act
            var result = await _service.DeleteAsync("999");

            // Assert
            Assert.False(result);
            _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<Note>()), Times.Never);
        }
    }
}
