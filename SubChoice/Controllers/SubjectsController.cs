using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.Services;

namespace SubChoice.Controllers
{
    public class SubjectsController : Controller
    {
        private ISubjectService _subjectService;
        private UserManager<User> _userManager;

        public SubjectsController(ISubjectService subjectService, UserManager<User> userManager)
        {
            _subjectService = subjectService;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var subjects = await _subjectService.SelectAllByTeacherId(user.Id);
            return View(subjects);
        }
    }
}
