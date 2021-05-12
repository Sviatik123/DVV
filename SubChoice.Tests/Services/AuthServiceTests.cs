using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.Services;
using SubChoice.Services;
using Xunit;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SubChoice.Tests.Services
{
    public class AuthServiceTests
    {
        IAuthService authService;
        Mock<UserManager<User>> userManager;
        Mock<SignInManager<User>> signInManager;
        Mock<IRepoWrapper> repo;
        Mock<IMapper> mapper;


        public AuthServiceTests()
        {
            var userStore = new Mock<IUserStore<User>>();
            this.userManager = new Mock<UserManager<User>>(
                userStore.Object,
                null, new PasswordHasher<User>(),
                null, null, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<UserManager<User>>>().Object);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            signInManager = new Mock<SignInManager<User>>(
                userManager.Object,
                contextAccessor.Object, userPrincipalFactory.Object, null, null, null, null);

            this.mapper = new Mock<IMapper>();
            this.repo = new Mock<IRepoWrapper>();
            this.authService = new AuthService(userManager.Object, signInManager.Object, mapper.Object, repo.Object);
        }

        [Fact]
        public async Task SignIn_TestAsync()
        {
            // Arrange
            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());
            signInManager
                .Setup(x => x.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>(),
                    It.IsAny<bool>())).ReturnsAsync(SignInResult.Success);

            // Act
            var result = await authService.SignInAsync(new LoginDto());

            // Assert
            userManager.Verify();
            signInManager.Verify();
            Assert.NotNull(result);
            Assert.IsType<SignInResult>(result);
            Assert.True(result.Succeeded);
        }

        [Fact]
        public async Task SignOut_TestAsync()
        {
            // Arrange
            signInManager
                .Setup(x => x.SignOutAsync());

            // Act
            await authService.SignOutAsync();

            // Assert
            signInManager.Verify();
        }

        [Fact]
        public async Task AddRole_TestAsync()
        {
            // Arrange
            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new User());
            userManager.Setup(x => x.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await authService.AddRoleAsync(new RegisterDto());

            // Assert
            userManager.Verify();
            Assert.NotNull(result);
            Assert.IsType<IdentityResult>(result);
            Assert.True(result.Succeeded);
        }


        //[Fact]
        //public async Task CreateUser_TestAsync()
        //{
        //    // Arrange
        //    mapper.Setup(x => x.Map<RegisterDto, User>(It.IsAny<RegisterDto>())).Returns(new User());
        //    userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        //    // Act
        //    var result = await authService.CreateUserAsync(new RegisterDto()
        //    {
        //        Email = "maxym@gmail.com",
        //        FirstName = "maxym",
        //        LastName = "maxym",
        //        Password = "maxym123",
        //        ConfirmPassword = "maxym123",
        //    });

        //    // Assert
        //    mapper.Verify();
        //    userManager.Verify();
        //    Assert.NotNull(result);
        //    Assert.IsType<IdentityResult>(result);
        //    Assert.True(result.Succeeded);
        //}
    }
}
