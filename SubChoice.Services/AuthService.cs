using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.Services;

namespace SubChoice.Services
{
    public class AuthService: IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IMapper _mapper;
        private IRepoWrapper _repository;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IRepoWrapper repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<SignInResult> SignInAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, true);
            return result;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterDto registerDto)
        {
            var user = _mapper.Map<RegisterDto, User>(registerDto);
            user.UserName = registerDto.Email;
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            return result;
        }

        public async Task<IdentityResult> AddRoleAsync(RegisterDto registerDto)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            return await _userManager.AddToRoleAsync(user, registerDto.Role);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public Teacher CreateTeacher(User user)
        {
            Teacher teacher = new Teacher();
            teacher.User = user; 
            return  _repository.Teachers.Create(teacher);
        }
    }
}
