using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SubChoice.Core.Configuration;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.Services;
using SubChoice.Services;

namespace SubChoice.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private IAuthService _authService;
        List<string> roles = new List<string>() { Roles.Student, Roles.Teacher };
        private ILoggerService _loggerService;

        public AuthController(IAuthService authService, ILoggerService loggerService)
        {
            _authService = authService;
            _loggerService = loggerService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            SelectList rolesList = new SelectList(roles);
            ViewBag.Roles = rolesList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var resultOfCreation = await _authService.CreateUserAsync(model);
                if (resultOfCreation.Succeeded)
                {
                    var resultOfAddToRole = await _authService.AddRoleAsync(model);
                    if (resultOfAddToRole.Succeeded)
                    {
                        try
                        {
                            var resultOfSignIn = await _authService.SignInAsync(new LoginDto()
                            { Email = model.Email, Password = model.Password, RememberMe = false });
                            if (resultOfSignIn.Succeeded)
                            {
                                _loggerService.LogInfo($"User {@model.Email} registered");
                            }

                            return RedirectToAction("Index", "Home");
                        }
                        catch
                        {
                            _loggerService.LogError($"User {@model.Email} registered but not approved");
                            ViewBag.ErrorTitle = $"{model.Email} is not approved";
                            ViewBag.ErrorMessage = $"You want to log in as a teacher. We need to check your email. Please, wait for an admin to approve it.";
                            return View("Error");
                        }
                    }
                }

                _loggerService.LogError($"Invalid login or password");
                ModelState.AddModelError(string.Empty, "Invalid login or password");
            }

            SelectList rolesList = new SelectList(roles);
            ViewBag.Roles = rolesList;
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _authService.SignInAsync(model);
                    if (result.Succeeded)
                    {
                        _loggerService.LogInfo($"User {@model.Email} logged in");
                        return RedirectToAction("Welcome", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "Invalid login or password");
                    _loggerService.LogError($"Invalid login or password");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Account is not approved")
                {
                    _loggerService.LogError($"Account {model.Email} is not approved");
                    ViewBag.ErrorTitle = $"{model.Email} is not approved";
                    ViewBag.ErrorMessage = $"You want to log in as a teacher. We need to check your email. Please, wait for an admin to approve it.";
                    return View("Error");
                }
                ModelState.AddModelError(string.Empty, "Invalid login or password");
                _loggerService.LogError($"Invalid login or password");
            }

            return View(model);
        }
    }
}
