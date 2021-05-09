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
        private readonly ILoggerService _loggerService;
        private ISubjectService _subjectService;
        private IAuthService _authService;
        UserManager<User> _userManager;

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
            else if (user.Student != null ){
                subjects = _subjectService.SelectAllByStudentId(user.Id).Result;
            }
            ViewData["Subjects"] = subjects;
            return View();
        }

        public IActionResult Create() {
            // TODO: Popagate techer to view 
            return View();
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

        [HttpGet]
        public IActionResult Admin()
        {
            var teachers = _subjectService.SelectNotApprovedTeachers().Result;
            ViewData["Teachers"] = teachers;
            return View("Admin");
        }

        public async Task<IActionResult> Admin(IdDto model)
        {
            if (!ModelState.IsValid)
            {
                _loggerService.LogError($"Error happened. Try again");
                return View();
            }
            ApproveUserDto data = new ApproveUserDto();
            var approvedTeacher = _subjectService.ApproveUser(model.Id);
            if (approvedTeacher == null)
            {
                _loggerService.LogError($"Not valid user id. Try again");
                ModelState.AddModelError(string.Empty, "Invalid login or password");
            }
            return View();
        }
    }
}
