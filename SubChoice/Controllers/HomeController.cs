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
    [Authorize(Roles = "Administrator, Student, Teacher")]
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

        public async Task<IActionResult> Index()
        {
            ViewData["Subjects"] = await _subjectService.SelectAllSubjects();
            return View("Subjects");
        }

        public async Task<IActionResult> ChosenSubjects()
        {
            var user = await _userManager.GetUserAsync(User);
            List<Subject> subjects = new List<Subject>();
            subjects = await _subjectService.SelectAllByStudentId(user.Id);
            ViewData["ChosenSubjects"] = subjects;
            return View();
        }

        [Authorize(Roles = "Administrator, Teacher")]
        public async Task<IActionResult> MySubjects()
        {
            var teacherId = await _userManager.GetUserAsync(User);
            ViewData["MySubjects"] = await _subjectService.SelectAllByTeacherId(teacherId.Id);
            return View("MySubjects");
        }

        [Authorize(Roles = "Administrator, Teacher")]

        public async Task<IActionResult> Create()
        {
            ViewData["TeacherId"] = (await _userManager.GetUserAsync(User)).Id;
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

        [HttpGet]
        public async Task<IActionResult> EditSubject(int id)
        {
            ViewData["TeacherId"] = (await _userManager.GetUserAsync(User)).Id;
            ViewData["Id"] = id;
            var subject = await _subjectService.SelectById(id);
            ViewData["Subject"] = subject;
            return View("EditSubject");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubject(SubjectData model)
        {

            if (ModelState.IsValid)
            {
                var updatedSubject = await _subjectService.UpdateSubject(model.Id, model);
                if (updatedSubject == null)
                {
                    _loggerService.LogError($"Fail to update subject");
                    ModelState.AddModelError(string.Empty, "Fail to update subject");
                }
                _loggerService.LogInfo($"Subject {model.Name} successfully updated.");
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

            var teacherId = _userManager.GetUserAsync(User).Result.Id;
            ViewData["MySubjects"] = _subjectService.SelectAllByTeacherId(teacherId).Result;
            return View("MySubjects");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(SubjectIdDto subId)
        {
            var studentId = _userManager.GetUserAsync(User).Result.Id;
            if (ModelState.IsValid)
            {
                var registered = await _subjectService.RegisterStudent(studentId, subId.Id);
                if (registered == null)
                {
                    _loggerService.LogError($"Fail to register User {studentId} on subject {subId.Id}");
                    ModelState.AddModelError(string.Empty, "Fail to register User on subject");
                }
                _loggerService.LogInfo($"User {studentId} sucessfully registered on subject {subId.Id}");
            }

            var subjects = _subjectService.SelectAllByStudentId(studentId).Result;
            ViewData["ChosenSubjects"] = subjects;
            return View("ChosenSubjects");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unregister(SubjectIdDto subId)
        {
            var studentId = _userManager.GetUserAsync(User).Result.Id;
            if (ModelState.IsValid)
            {
                var unregistered = await _subjectService.UnRegisterStudent(studentId, subId.Id);
                if (unregistered == null)
                {
                    _loggerService.LogError($"Fail to unregister User {studentId} on subject {subId.Id}");
                    ModelState.AddModelError(string.Empty, "Fail to unregister User on subject");
                }
                _loggerService.LogInfo($"User {studentId} sucessfully unregistered on subject {subId.Id}");
            }

            var subjects = _subjectService.SelectAllByStudentId(studentId).Result;
            ViewData["ChosenSubjects"] = subjects;
            return View("ChosenSubjects");
        }

        [HttpGet]
        public async Task<IActionResult> StudentsSubject(int id)
        {
            var students = await _subjectService.SelectAllStudentsSubjects(id);
            ViewData["StudentsSubject"] = students;
            return View("StudentsSubject");
        }

        [HttpGet]
        public async Task<IActionResult> SubjectDetail(int id)
        {
            var subject = await _subjectService.SelectById(id);
            ViewData["Subject"] = subject;
            return View("SubjectDetail");
        }

        [HttpGet]
        public  IActionResult Welcome()
        {
            return View("Welcome");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
