using Moq;
using Moq.Protected;
using System.Net;
using Microsoft.Extensions.Logging;
using P10.Frontend.Services;

namespace FrontendTests
{
    public class AssessmentApiClientTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<AssessmentApiClient>> _mockLogger;
        private readonly AssessmentApiClient _client;

        public AssessmentApiClientTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:7050")
            };
            _mockLogger = new Mock<ILogger<AssessmentApiClient>>();
            _client = new AssessmentApiClient(_httpClient, _mockLogger.Object);
        }

        [Fact]
        public async Task GetPatientRiskAsync_ValidPatientId_ReturnsRiskLevel()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("InDanger")
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _client.GetPatientRiskAsync(1, "fake-token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("InDanger", result);
        }

        [Fact]
        public async Task GetPatientRiskAsync_PatientNotFound_ReturnsNull()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _client.GetPatientRiskAsync(999, "fake-token");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPatientRiskAsync_UnauthorizedRequest_ReturnsNull()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized
            };

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _client.GetPatientRiskAsync(1, "invalid-token");

            // Assert
            Assert.Null(result);
        }
    }
}
