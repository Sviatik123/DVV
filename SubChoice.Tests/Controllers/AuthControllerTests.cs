using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SubChoice.Controllers;
using SubChoice.Core.Configuration;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Interfaces.Services;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SubChoice.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _authController;
        private readonly Mock<IAuthService> _authService;
        private readonly Mock<ILoggerService> _loggerService;

        public AuthControllerTests()
        {
            _authService = new Mock<IAuthService>();
            _loggerService = new Mock<ILoggerService>();
            _authController = new AuthController(_authService.Object, _loggerService.Object);
        }

        [Fact]
        public async Task LoginPost_SignInNotSucceeded_TestAsync()
        {
            // Arrange
            _authService.Setup(x => x.SignInAsync(It.IsAny<LoginDto>())).ReturnsAsync(new SignInResult());

            // Act
            var result = await _authController.Login(new LoginDto());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task LoginPost_SignInSucceeded_TestAsync()
        {
            // Arrange
            _authService.Setup(x => x.SignInAsync(It.IsAny<LoginDto>())).ReturnsAsync(SignInResult.Success);

            // Act
            var result = await _authController.Login(new LoginDto());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task LoginPost_InvalidModelState_TestAsync()
        {
            // Arrange
            _authService.Setup(x => x.SignInAsync(It.IsAny<LoginDto>())).ReturnsAsync(SignInResult.Success);

            // Act
            _authController.ModelState.AddModelError(string.Empty, "some error");
            var result = await _authController.Login(new LoginDto());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task RegisterPost_RegisterNotSucceeded_TestAsync()
        {
            // Arrange
            _authService.Setup(x => x.CreateUserAsync(It.IsAny<RegisterDto>())).ReturnsAsync(new IdentityResult());

            // Act
            var result = await _authController.Register(new RegisterDto());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task RegisterPost_InvalidModelState_TestAsync()
        {
            // Arrange
            _authService.Setup(x => x.CreateUserAsync(It.IsAny<RegisterDto>())).ReturnsAsync(IdentityResult.Success);

            // Act
            _authController.ModelState.AddModelError(string.Empty, "some error");
            var result = await _authController.Register(new RegisterDto());

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
