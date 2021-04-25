using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SubChoice.Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using SubChoice.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SubChoice.Core.Data.Entities;

namespace SubChoice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISubjectService _subjectService;
        private IAuthService _authService;
        UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, ISubjectService subjectService, IAuthService authService, UserManager<User> userManager)
        {
            _logger = logger;
            _subjectService = subjectService;
            _authService = authService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            ViewData["Subjects"] = _subjectService.SelectAllSubjects().Result;
            return View("Subjects");
        }

        public IActionResult MySubjects()
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
    }
}
