using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace SubChoice.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private ISubjectService _subjectService;
        private ILoggerService _loggerService;
        private IAuthService _authService;

        public AdminController(ISubjectService subjectService, ILoggerService loggerService, IAuthService authService)
        {
            _subjectService = subjectService;
            _loggerService = loggerService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var teachers = await _subjectService.SelectNotApprovedTeachers();
            ViewData["Teachers"] = teachers;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllSubjects()
        {
            ViewData["Subjects"] = await _subjectService.SelectAllSubjects();
            return View("Subjects");
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            ViewData["Users"] = await _authService.GetUsers();
            return View("Users");
        }

        [HttpPost]
        public async Task<IActionResult> ApproveTeacher(IdDto model)
        {
            if (!ModelState.IsValid)
            {
                _loggerService.LogError($"Error happened. Try again");
                return View("Index");
            }
            ApproveUserDto data = new ApproveUserDto();
            var approvedTeacher = await _subjectService.ApproveUser(model.Id);
            if (approvedTeacher == null)
            {
                _loggerService.LogError($"Not valid user id. Try again");
                ModelState.AddModelError(string.Empty, "Invalid login or password");
            }

            var teachers = await _subjectService.SelectNotApprovedTeachers();
            ViewData["Teachers"] = teachers;
            return View("Index");
        }
    }
}
