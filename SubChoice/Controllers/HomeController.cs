using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.Services;
using SubChoice.Models;

namespace SubChoice.Controllers
{
    [Authorize(Roles = "Admin, Student, Teacher")]
    public class HomeController : Controller
    {
        private ISubjectService _subjectService;
        private IAuthService _authService;
        UserManager<User> _userManager;
        private ILoggerService _loggerService;

        public HomeController(ILoggerService loggerService, ISubjectService subjectService, IAuthService authService, UserManager<User> userManager)
        {
            _loggerService = loggerService;
            _subjectService = subjectService;
            _authService = authService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewData["Subjects"] = _subjectService.SelectAllSubjects().Result;
            return View("Subjects");
        }

        public IActionResult ChosenSubjects()
        {
            var user = _userManager.GetUserAsync(User).Result;
            List<Subject> subjects = new List<Subject>();
            if (user.Teacher != null)
            {
               subjects = _subjectService.SelectAllByTeacherId(user.Id).Result;
            }
            else if (user.Student != null ) {
                subjects = _subjectService.SelectAllByStudentId(user.Id).Result;
            }
            ViewData["ChosenSubjects"] = subjects;
            return View();
        }

        [Authorize(Roles = "Admin, Teacher")]
        public IActionResult MySubjects()
        {
            var teacherId = _userManager.GetUserAsync(User).Result.Id;
            ViewData["MySubjects"] = _subjectService.SelectAllByTeacherId(teacherId).Result;
            return View("MySubjects");
        }

        [Authorize(Roles = "Admin, Teacher")]

        public IActionResult Create()
        {
            ViewData["TeacherId"] = _userManager.GetUserAsync(User).Result.Id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubjectData model)
        {

            if (ModelState.IsValid)
            {
                var createdSubject = await _subjectService.CreateSubject(model);
                if (createdSubject == null)
                {
                    _loggerService.LogError($"Invalid login or password");
                    ModelState.AddModelError(string.Empty, "Invalid login or password");
                }
                _loggerService.LogInfo($"Invalid login or password");
            }

            //SelectList rolesList = new SelectList(roles);
            //ViewBag.Roles = rolesList;
            return View("MySubjects");
        }

        public IActionResult SubjectDetail(int id)
        {
            var subject = _subjectService.SelectById(id);
            ViewData["Subject"] = subject;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
