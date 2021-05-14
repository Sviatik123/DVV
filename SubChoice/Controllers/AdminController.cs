using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using SubChoice.Core.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace SubChoice.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private ISubjectService _subjectService;
        private ILoggerService _loggerService;
        UserManager<User> _userManager;
        private IAuthService _authService;

        public AdminController(ISubjectService subjectService, ILoggerService loggerService, IAuthService authService, UserManager<User> userManager)
        {
            _subjectService = subjectService;
            _loggerService = loggerService;
            _authService = authService;
            _userManager = userManager;
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

        public async Task<IActionResult> Create()
        {
            //ViewData["Teachers"] = (await _authService.GetTeachers);
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
                    _loggerService.LogError($"Fail to create subject {model.Name} by {model.TeacherId}");
                    ModelState.AddModelError(string.Empty, "Invalid login or password");
                }
                _loggerService.LogInfo($"Subject {model.Name} successfully created by {model.TeacherId}");
            }

            var teacherId = _userManager.GetUserAsync(User).Result.Id;
            ViewData["MySubjects"] = _subjectService.SelectAllByTeacherId(teacherId).Result;
            return View("MySubjects");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubject(SubjectIdDto model)
        {
            if (ModelState.IsValid)
            {
                var deleteSubject = await _subjectService.DeleteSubject(model.Id);

                if (deleteSubject == null)
                {
                    _loggerService.LogError($"Fail to delete subject {model.Id}");
                    ModelState.AddModelError(string.Empty, "Fail to delete subject");
                }
                _loggerService.LogInfo($"Successfully delete subject {model.Id}");
            }

            ViewData["Subjects"] = await _subjectService.SelectAllSubjects();
            return View("Subjects");
        }
    }
}
