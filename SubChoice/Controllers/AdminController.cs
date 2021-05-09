using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SubChoice.Core.Data.Dto;
using SubChoice.Core.Interfaces.Services;

namespace SubChoice.Controllers
{
    public class AdminController : Controller
    {
        private ISubjectService _subjectService;
        private ILoggerService _loggerService;

        public AdminController(ISubjectService subjectService, ILoggerService loggerService)
        {
            _subjectService = subjectService;
            _loggerService = loggerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var teachers = _subjectService.SelectNotApprovedTeachers().Result;
            ViewData["Teachers"] = teachers;
            return View();
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
            return View("Index");
        }
    }
}
