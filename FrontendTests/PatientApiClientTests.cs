using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using P10.Frontend.Models;
using P10.Frontend.Services;

namespace FrontendTests
{
    public class PatientApiClientTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<PatientApiClient>> _mockLogger;
        private readonly PatientApiClient _client;

        public PatientApiClientTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:7050")
            };
            _mockLogger = new Mock<ILogger<PatientApiClient>>();
            _client = new PatientApiClient(_httpClient, _mockLogger.Object);
        }

        [Fact]
        public async Task GetPatientsAsync_ValidToken_ReturnsPatients()
        {
            // Arrange
            var patients = new List<PatientViewModel>
            {
                new PatientViewModel { Id = 1, FirstName = "John", LastName = "Doe" },
                new PatientViewModel { Id = 2, FirstName = "Jane", LastName = "Smith" }
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(patients)
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _client.GetPatientsAsync("fake-token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ValidId_ReturnsPatient()
        {
            // Arrange
            var patient = new PatientViewModel { Id = 1, FirstName = "John", LastName = "Doe" };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(patient)
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _client.GetPatientByIdAsync(1, "fake-token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("John", result.FirstName);
        }

        [Fact]
        public async Task CreatePatientAsync_ValidPatient_ReturnsTrue()
        {
            // Arrange
            var patient = new PatientViewModel { FirstName = "John", LastName = "Doe" };

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
            var result = await _client.CreatePatientAsync(patient, "fake-token");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeletePatientAsync_ExistingPatient_ReturnsTrue()
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
            var result = await _client.DeletePatientAsync(1, "fake-token");

            // Assert
            Assert.True(result);
        }
    }
}
