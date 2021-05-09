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
    public class AdminCotroller : Controller
    {
        private ISubjectService _subjectService;

        public AdminCotroller(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var teachers = _subjectService.SelectNotApprovedTeachers().Result;
            ViewData["Teachers"] = teachers;
            return View("Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveTeacher(RegisterDto model)
        {
            return null;
        }
    }
}
