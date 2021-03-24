using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SubChoice.Controllers;
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

        public AuthControllerTests()
        {
            _authService = new Mock<IAuthService>();
            _authController = new AuthController(_authService.Object);
        }

        [Fact]
        public async Task LoginPost_SignInNotSucceeded_TestAsync()
        {
            //Arrange
            _authService.Setup(x => x.SignInAsync(It.IsAny<LoginDto>())).ReturnsAsync(new SignInResult());

            //Act
            var result = await _authController.Login(new LoginDto());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task LoginPost_SignInSucceeded_TestAsync()
        {
            //Arrange
            _authService.Setup(x => x.SignInAsync(It.IsAny<LoginDto>())).ReturnsAsync(SignInResult.Success);

            //Act
            var result = await _authController.Login(new LoginDto());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public async Task LoginPost_InvalidModelState_TestAsync()
        {
            //Arrange
            _authService.Setup(x => x.SignInAsync(It.IsAny<LoginDto>())).ReturnsAsync(SignInResult.Success);

            //Act
            _authController.ModelState.AddModelError("", "some error");
            var result = await _authController.Login(new LoginDto());

            //Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

    }
}
