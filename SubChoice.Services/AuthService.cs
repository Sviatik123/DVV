using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.Services;

namespace SubChoice.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IMapper _mapper;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._mapper = mapper;
        }

        public async Task<SignInResult> SignInAsync(LoginDto loginDto)
        {
            var user = await this._userManager.FindByEmailAsync(loginDto.Email);
            var result = await this._signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, true);
            return result;
        }

        public async Task SignOutAsync()
        {
            await this._signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterDto registerDto)
        {
            var user = this._mapper.Map<RegisterDto, User>(registerDto);
            user.UserName = registerDto.Email;
            var result = await this._userManager.CreateAsync(user, registerDto.Password);
            return result;
        }

        public async Task<IdentityResult> AddRoleAsync(RegisterDto registerDto)
        {
            var user = await this._userManager.FindByEmailAsync(registerDto.Email);
            return await this._userManager.AddToRoleAsync(user, registerDto.Role);
        }
    }
}
