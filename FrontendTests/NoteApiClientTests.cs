using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using P10.Frontend.Models;
using P10.Frontend.Services;

namespace FrontendTests
{
    public class NoteApiClientTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<NoteApiClient>> _mockLogger;
        private readonly NoteApiClient _client;

        public NoteApiClientTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:7050")
            };
            _mockLogger = new Mock<ILogger<NoteApiClient>>();
            _client = new NoteApiClient(_httpClient, _mockLogger.Object);
        }

        [Fact]
        public async Task GetNotesByPatientAsync_ValidPatientId_ReturnsNotes()
        {
            // Arrange
            var notes = new List<NoteViewModel>
            {
                new NoteViewModel { Id = "1", PatientId = 1, Content = "Note 1" },
                new NoteViewModel { Id = "2", PatientId = 1, Content = "Note 2" }
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(notes)
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _client.GetNotesByPatientAsync(1, "fake-token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, n => Assert.Equal(1, n.PatientId));
        }

        [Fact]
        public async Task GetNoteByIdAsync_ValidId_ReturnsNote()
        {
            // Arrange
            var note = new NoteViewModel { Id = "1", PatientId = 1, Content = "Test Note" };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(note)
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _client.GetNoteByIdAsync("1", "fake-token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
            Assert.Equal("Test Note", result.Content);
        }

        [Fact]
        public async Task CreateNoteAsync_ValidNote_ReturnsTrue()
        {
            // Arrange
            var note = new NoteViewModel { PatientId = 1, Content = "New Note" };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _client.CreateNoteAsync(note, "fake-token");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateNoteAsync_ValidNote_ReturnsTrue()
        {
            // Arrange
            var note = new NoteViewModel { Id = "1", PatientId = 1, Content = "Updated Note" };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _client.UpdateNoteAsync(note, "fake-token");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteNoteAsync_ExistingNote_ReturnsTrue()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _client.DeleteNoteAsync("1", "fake-token");

            // Assert
            Assert.True(result);
        }
    }
}
