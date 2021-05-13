using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Data.Entities;

namespace SubChoice.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<SignInResult> SignInAsync(LoginDto loginDto);

        Task SignOutAsync();

        Task<IdentityResult> CreateUserAsync(RegisterDto registerDto);

        Task<IdentityResult> AddRoleAsync(RegisterDto registerDto);

        Task<User> GetUserByEmail(string email);

        Task<Dictionary<string, List<User>>> GetUsers();
    }
}
