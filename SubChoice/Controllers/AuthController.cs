using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using SubChoice.Core.Configuration;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.Services;

namespace SubChoice.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private IAuthService _authService;
        List<string> roles = new List<string>() { Roles.Student, Roles.Teacher };
        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
                        var resultOfSignIn = await _authService.SignInAsync(new LoginDto()
                            {Email = model.Email, Password = model.Password, RememberMe = false});
                        if (resultOfSignIn.Succeeded)
                            return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Invalid login or password");
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
            if (ModelState.IsValid)
            {
                var result = await _authService.SignInAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login or password");
            }
            return View(model);
        }

    }
}
