using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyManager.Contracts;
using AcademyManager.Models;
using AcademyManager.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademyManager.Controllers
{
    public class FacilitatorsController : Controller
    {
        private readonly UserManager<AMUser> _userManager;
        private readonly SignInManager<AMUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ICoursesRepository _coursesRepository;
        private readonly ITestsAndExamsRepository _testsAndExamsRepository;

        public FacilitatorsController(UserManager<AMUser> userManager, SignInManager<AMUser> signInManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper, ICoursesRepository coursesRepository, ITestsAndExamsRepository testsAndExamsRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _coursesRepository = coursesRepository;
            _testsAndExamsRepository = testsAndExamsRepository;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Facilitator");
            var model = _mapper.Map<IList<AMUser>, IList<FacilitatorsVM>>(users);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateFacilitators(int id)
        {
            var appUsers = _userManager.Users.AsEnumerable().Where(p => p.UserName != "admin@localhost.com").ToList();
            List<AMUser> toBeSelected = new List<AMUser>();
            for (int i = 0; i < appUsers.Count; i++)
            {
                if (!(await _userManager.IsInRoleAsync(appUsers[i], "Trainee"))
                    && !(await _userManager.IsInRoleAsync(appUsers[i], "Facilitator")))
                {
                    toBeSelected.Add(appUsers[i]);
                    continue;
                }
                else
                    continue;
            }
            var model = _mapper.Map<List<AMUser>, List<FacilitatorsVM>>(toBeSelected);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFacilitators(List<FacilitatorsVM> model)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    if (model[i].IsSelected)
                    {
                        var user = await _userManager.FindByIdAsync(model[i].Id);
                        var result = await _userManager.AddToRoleAsync(user, "Facilitator");
                        if (result.Succeeded)
                        {
                            continue;
                        }
                        else if (!(result.Succeeded))
                        {
                            ModelState.AddModelError("", "An error occured while creating the facilitators");
                            return View(model);
                        }
                    }
                    else
                        continue;
                }
                return RedirectToAction("Index", "AdminPortal");
            }
            else
            {
                ModelState.AddModelError("", "An error occured");
                return View(model);
            }
        }

        public IActionResult ManageCourses()
        {
            var user = _userManager.GetUserAsync(User).Result;
            ViewBag.Id = user.Id;
            return View();
        }

        public IActionResult FacilitatorCourses(string userId)
        {
            var courses = _coursesRepository.GetCourseByFacilitatorId(userId).ToList();
            if (courses.Count > 0)
            {
                var model = _mapper.Map<List<CourseVM>>(courses);
                return View(model);
            }
            return RedirectToAction("Error","Home");
        }

        public IActionResult AddTestOrExam(int id)
        {
            var course = _coursesRepository.FindById(id);
            var courseModel = _mapper.Map<CourseVM>(course);
            var model = new TestAndExamVM
            {
                CourseId = id,
                Course = courseModel
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddTestOrExam(TestAndExamVM model)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.GetUserAsync(User).Result;
                var newTestOrExam = new TestsAndExams
                {
                    CourseId = model.CourseId,
                    TestOrExamName = model.TestOrExamName,
                    Total = model.Total
                };
                var isCreated = _testsAndExamsRepository.Create(newTestOrExam);
                if (isCreated)
                {
                    return RedirectToAction("FacilitatorCourses", new { userId = user.Id });
                }
                else
                {
                    return View("Error", "Home");
                }
                
            }
            ModelState.AddModelError("", "Please fill out the form appropriately");
            return View(model);
        }

        public IActionResult ViewCourseDetails(int courseId)
        {
            var course = _coursesRepository.FindById(courseId);
            if (course != null)
            {
                var model = _mapper.Map<CourseVM>(course);
                return View(model);
            }
            return View("Error", "Home");
        }

        public IActionResult ViewCourseTestsAndExams(int courseId)
        {
            var courseTestsAndExams = _testsAndExamsRepository.GetTestsAndExamsByCourseId(courseId).ToList();
            var model = _mapper.Map<List<TestAndExamVM>>(courseTestsAndExams);
            return View(model);
        }
    }
}