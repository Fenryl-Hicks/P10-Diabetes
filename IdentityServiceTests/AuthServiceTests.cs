using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using IdentityService.Models;
using IdentityService.Repositories.Interfaces;
using IdentityService.Services;

namespace IdentityServiceTests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IOptions<JwtOptions>> _mockJwtOptions;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockJwtOptions = new Mock<IOptions<JwtOptions>>();

            var jwtOptions = new JwtOptions
            {
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                SigningKey = "super_secret_dev_key_change_me_please_32_chars_min"
            };
            _mockJwtOptions.Setup(o => o.Value).Returns(jwtOptions);

            _service = new AuthService(_mockUserRepository.Object, _mockJwtOptions.Object);
        }

        [Fact]
        public async Task GenerateTokenAsync_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var user = new IdentityUser
            {
                Id = "user123",
                UserName = "testuser",
                Email = "test@example.com"
            };

            _mockUserRepository.Setup(r => r.FindByUsernameAsync("testuser")).ReturnsAsync(user);
            _mockUserRepository.Setup(r => r.CheckPasswordAsync(user, "Password123!")).ReturnsAsync(true);

            // Act
            var result = await _service.GenerateTokenAsync("testuser", "Password123!");

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains(".", result); // JWT has dots separating parts
        }

        [Fact]
        public async Task GenerateTokenAsync_InvalidUsername_ReturnsNull()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.FindByUsernameAsync("nonexistent")).ReturnsAsync((IdentityUser?)null);

            // Act
            var result = await _service.GenerateTokenAsync("nonexistent", "Password123!");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GenerateTokenAsync_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var user = new IdentityUser
            {
                Id = "user123",
                UserName = "testuser",
                Email = "test@example.com"
            };

            _mockUserRepository.Setup(r => r.FindByUsernameAsync("testuser")).ReturnsAsync(user);
            _mockUserRepository.Setup(r => r.CheckPasswordAsync(user, "WrongPassword")).ReturnsAsync(false);

            // Act
            var result = await _service.GenerateTokenAsync("testuser", "WrongPassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RegisterUserAsync_ValidUser_ReturnsSuccess()
        {
            // Arrange
            var identityResult = IdentityResult.Success;
            _mockUserRepository.Setup(r => r.CreateAsync(It.IsAny<IdentityUser>(), "Password123!"))
                .ReturnsAsync(identityResult);

            // Act
            var result = await _service.RegisterUserAsync("newuser", "new@example.com", "Password123!");

            // Assert
            Assert.True(result.Succeeded);
            _mockUserRepository.Verify(r => r.CreateAsync(It.IsAny<IdentityUser>(), "Password123!"), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_UserAlreadyExists_ReturnsFailure()
        {
            // Arrange
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "User already exists" });
            _mockUserRepository.Setup(r => r.CreateAsync(It.IsAny<IdentityUser>(), "Password123!"))
                .ReturnsAsync(identityResult);

            // Act
            var result = await _service.RegisterUserAsync("existinguser", "existing@example.com", "Password123!");

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "User already exists");
        }

        [Fact]
        public async Task RegisterUserAsync_WeakPassword_ReturnsFailure()
        {
            // Arrange
            var identityResult = IdentityResult.Failed(new IdentityError { Description = "Password too weak" });
            _mockUserRepository.Setup(r => r.CreateAsync(It.IsAny<IdentityUser>(), "weak"))
                .ReturnsAsync(identityResult);

            // Act
            var result = await _service.RegisterUserAsync("newuser", "new@example.com", "weak");

            // Assert
            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "Password too weak");
        }
    }
}
