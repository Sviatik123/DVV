﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SubChoice.Core.Configuration;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess;
using SubChoice.Core.Interfaces.Services;

namespace SubChoice.Services
{
    public class AuthService : IAuthService
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
            var user = await this._userManager.FindByEmailAsync(loginDto.Email);

            if (user.IsApproved == false)
            {
                throw new Exception("Account is not approved");
            }

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

            if (registerDto.Role == Roles.Teacher)
            {
                //user.IsApproved = false;
            }

            var result = await this._userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                var createdUser = await _userManager.FindByEmailAsync(registerDto.Email);
                var userId = createdUser.Id;
                Guid guid = Guid.NewGuid();

                if (registerDto.Role == Roles.Student)
                {
                    Student student = new Student() { User = createdUser, Id = guid, UserId = userId, StudentSubjects = new List<StudentSubject>() };
                    _repository.Students.Create(student);
                    _repository.SaveChanges();

                }
                else if (registerDto.Role == Roles.Teacher)
                {
                    Teacher teacher = new Teacher() { User = createdUser, Id = guid, UserId = userId, Subjects = new List<Subject>() };
                    _repository.Teachers.Create(teacher);
                    _repository.SaveChanges();
                }
            }

            return result;
        }

        public async Task<IdentityResult> AddRoleAsync(RegisterDto registerDto)
        {
            var user = await this._userManager.FindByEmailAsync(registerDto.Email);
            return await this._userManager.AddToRoleAsync(user, registerDto.Role);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public Teacher CreateTeacher(User user)
        {
            Teacher teacher = new Teacher();
            teacher.User = user; 
            return _repository.Teachers.Create(teacher);
        }
    }
}
