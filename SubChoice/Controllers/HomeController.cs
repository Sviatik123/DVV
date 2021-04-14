using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SubChoice.Core.Interfaces.Services;
using SubChoice.Models;
using System.Diagnostics;

namespace SubChoice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISubjectService _subjectService;

        public HomeController(ILogger<HomeController> logger, ISubjectService subjectService)
        {
            _logger = logger;
            _subjectService = subjectService;
        }

        public IActionResult Index()
        {
            return View("Subjects");
        }

        public IActionResult MySubjects()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
