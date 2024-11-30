using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using job_buddy_backend.Controllers;
using job_buddy_backend.Core.Interfaces;
using job_buddy_backend.DTO;
using Microsoft.Extensions.Logging;
using job_buddy_backend.Models.UserModel;
using System.Threading.Tasks;
using System.Security.Claims;
using job_buddy_backend.Models;
using Microsoft.AspNetCore.Http;

namespace job_buddy_backend.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<ILogger<AuthController>> _mockLogger;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockEmailService = new Mock<IEmailService>();
            _mockLogger = new Mock<ILogger<AuthController>>();
            _controller = new AuthController(_mockAuthService.Object, _mockEmailService.Object, _mockLogger.Object);
        }

        #region Register Tests
        [Fact]
        public async Task Register_ShouldReturnOk_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registerDto = new RegisterUserDto
            {
                FullName = "Test User",
                Email = "test@example.com",
                Password = "Test@123",
                Role = "User"
            };

            var user = new User
            {
                UserID = 1,
                FullName = "Test User",
                Email = "test@example.com",
                Role = "User",
                IsEmailVerified = false
            };

            _mockAuthService.Setup(s => s.RegisterAsync(registerDto)).ReturnsAsync(user);

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<ApiResponse<UserDto>>(okResult.Value);

            response.Should().NotBeNull();
            response.Message.Should().Be("Registration successful. Please check your email to confirm your account.");
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Email", "Required");

            // Act
            var result = await _controller.Register(new RegisterUserDto());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsAssignableFrom<ApiResponse<string>>(badRequestResult.Value);

            response.Message.Should().Be("Invalid registration data.");
        }

        [Fact]
        public async Task Register_ShouldReturnServerError_WhenRegistrationFails()
        {
            // Arrange
            var registerDto = new RegisterUserDto
            {
                FullName = "Test User",
                Email = "test@example.com",
                Password = "Test@123",
                Role = "User"
            };

            _mockAuthService.Setup(s => s.RegisterAsync(registerDto)).ThrowsAsync(new InvalidOperationException("User with this email already exists."));

            // Act
            var result = await _controller.Register(registerDto);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            var response = Assert.IsAssignableFrom<ApiResponse<string>>(serverErrorResult.Value);

            response.Message.Should().Be("User with this email already exists.");
        }
        #endregion

        #region Login Tests
        [Fact]
        public async Task Login_ShouldReturnOk_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginDto = new LoginUserDto
            {
                Email = "test@example.com",
                Password = "Test@123"
            };

            var user = new User
            {
                UserID = 1,
                FullName = "Test User",
                Email = "test@example.com",
                Role = "User",
                IsEmailVerified = true
            };

            var token = "fake-jwt-token";

            _mockAuthService.Setup(s => s.LoginAsync(loginDto)).ReturnsAsync(user);
            _mockAuthService.Setup(s => s.GenerateJwtToken(user)).Returns(token);

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<ApiResponse<LoginResponseDto>>(okResult.Value);

            response.Should().NotBeNull();
            response.Message.Should().Be("Login successful.");
            response.Data.Token.Should().Be(token);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginDto = new LoginUserDto
            {
                Email = "invalid@example.com",
                Password = "WrongPassword"
            };

            _mockAuthService.Setup(s => s.LoginAsync(loginDto)).ThrowsAsync(new UnauthorizedAccessException("Invalid credentials! Please try again."));

            // Act
            var result = await _controller.Login(loginDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsAssignableFrom<ApiResponse<string>>(unauthorizedResult.Value);

            response.Message.Should().Be("Invalid credentials! Please try again.");
        }
        #endregion

        #region VerifyToken Tests
        [Fact]
        public void VerifyToken_ShouldReturnOk_WhenTokenIsValid()
        {
            // Arrange
            var user = new User
            {
                UserID = 1,
                Email = "test@example.com",
                FullName = "Test User",
                Role = "User",
                IsEmailVerified = true
            };

            var claims = new[]
            {
                new Claim("UserID", user.UserID.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var mockUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockUser }
            };

            _mockAuthService.Setup(s => s.GetUserByIdAsync(user.UserID)).ReturnsAsync(user);

            // Act
            var result = _controller.VerifyToken();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<ApiResponse<UserDto>>(okResult.Value);

            response.Should().NotBeNull();
            response.Message.Should().Be("Token is valid.");
            response.Data.Email.Should().Be(user.Email);
        }

        [Fact]
        public void VerifyToken_ShouldReturnUnauthorized_WhenTokenIsInvalid()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = _controller.VerifyToken();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsAssignableFrom<ApiResponse<string>>(unauthorizedResult.Value);

            response.Message.Should().Be("Invalid or expired token.");
        }
        #endregion
    }
}
