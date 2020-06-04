using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyManager.Contracts;
using AcademyManager.Models;
using AcademyManager.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademyManager.Controllers
{
    [Authorize(Roles = "Facilitator")]
    public class FacilitatorsController : Controller
    {
        private readonly UserManager<AMUser> _userManager;
        private readonly SignInManager<AMUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ICoursesRepository _coursesRepository;
        private readonly ITestsAndExamsRepository _testsAndExamsRepository;
        private readonly IScoresRepository _scoresRepository;

        public FacilitatorsController(UserManager<AMUser> userManager, SignInManager<AMUser> signInManager, 
            RoleManager<IdentityRole> roleManager,
            IMapper mapper, ICoursesRepository coursesRepository, ITestsAndExamsRepository testsAndExamsRepository,
            IScoresRepository scoresRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _coursesRepository = coursesRepository;
            _testsAndExamsRepository = testsAndExamsRepository;
            _scoresRepository = scoresRepository;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Facilitator");
            var model = _mapper.Map<IList<AMUser>, IList<FacilitatorsVM>>(users);
            return View(model);
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
            var model = _mapper.Map<List<CourseVM>>(courses);
            return View(model);
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

        [HttpGet]
        public IActionResult EditTestsOrExam(int testOrExamId)
        {
            var testOrExam = _testsAndExamsRepository.FindById(testOrExamId);
            if (testOrExam == null)
            {
                return View("Error", "Home");
            }
            var model = _mapper.Map<TestAndExamVM>(testOrExam);
            return View(model);
        }

        [HttpPost]
        public IActionResult EditTestsOrExam(TestAndExamVM model)
        {
            if (ModelState.IsValid)
            {
                var testOrExam = _testsAndExamsRepository.FindById(model.Id);
                if (testOrExam != null)
                {
                    testOrExam.TestOrExamName = model.TestOrExamName;
                    testOrExam.Total = model.Total;
                    var isSuccess = _testsAndExamsRepository.Update(testOrExam);
                    if (isSuccess)
                    {
                        return RedirectToAction("ViewCourseTestsAndExams", new { courseId = testOrExam.CourseId });
                    }
                }
                return View("Error", "Home");
            }
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
            var testsAndExams = _mapper.Map<List<TestAndExamVM>>(courseTestsAndExams);
            var model = new ViewCourseTestsAndExamsVM
            {
                CourseId = courseId,
                TestsAndExams = testsAndExams
            };
            return View(model);
        }

        public IActionResult ListTraineesForTest(int testOrExamId)
        {
            var trainees = _userManager.GetUsersInRoleAsync("Trainee").Result.ToList();
            var testOrExam = _testsAndExamsRepository.FindById(testOrExamId);
            var course = _coursesRepository.FindById(testOrExam.CourseId);
            var courseVM = _mapper.Map<CourseVM>(course);
            var testOrExamModel = _mapper.Map<TestAndExamVM>(testOrExam);
            testOrExamModel.Course = courseVM;
            var traineesModel = _mapper.Map<List<TraineeVM>>(trainees);
            var model = new ListTraineesForTestVM
            {
                TestOrExam = testOrExamModel,
                TraineesForTest = traineesModel
            };
            return View(model);
        }

        public IActionResult ViewTraineesScores(int testOrExamId)
        {
            var scoresforTestOrExam = _scoresRepository.GetScoreByTestOrExamId(testOrExamId).ToList();
            var model = _mapper.Map<List<ScoresVM>>(scoresforTestOrExam);
            return View(model);
        }

        public IActionResult AddScore(int testOrExamId, string traineeId)
        {
            var model = new ScoresVM
            {
                TestOrExamId = testOrExamId,
                TraineeId = traineeId
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddScore(ScoresVM model)
        {
            if (ModelState.IsValid)
            {
                var testOrExam = _testsAndExamsRepository.FindById(model.TestOrExamId);
                if (model.Score > testOrExam.Total)
                {
                    ModelState.AddModelError("", "The value of the trainee's score cannot be higher than the total score");
                    return View(model);
                }
                var score = _mapper.Map<Scores>(model);
                var isSuccess = _scoresRepository.Create(score);
                if (!isSuccess)
                {
                    return View("Error","Home");
                }
                return RedirectToAction("ListTraineesForTest", new { testOrExamId = score.TestOrExamId});
            }
            ModelState.AddModelError("", "FIll all the fields properly");
            return View(model);
        }

        public IActionResult EditScore(int testOrExamId, string traineeId)
        {
            var score = _scoresRepository.GetScoreByTestAndExamIdAndTraineeId(testOrExamId, traineeId);
            var model = _mapper.Map<ScoresVM>(score);
            return View(model);
        }

        [HttpPost]
        public IActionResult EditScore(ScoresVM model)
        {
            if (ModelState.IsValid)
            {
                var testOrExam = _testsAndExamsRepository.FindById(model.TestOrExamId);
                if (model.Score > testOrExam.Total)
                {
                    ModelState.AddModelError("", "The value of the trainee's score cannot be higher than the total score");
                    return View(model);
                }
                var score = _scoresRepository.FindById(model.Id);
                score.Score = model.Score;
                var isSuccess = _scoresRepository.Update(score);
                if (!isSuccess)
                {
                    return View("Error", "Home");
                }
                return RedirectToAction("ListTraineesForTest", new { testOrExamId = score.TestOrExamId });
            }
            ModelState.AddModelError("", "FIll all the fields properly");
            return View(model);
        }

        public IActionResult DeleteTestOrExam(int testOrExamId)
        {
            var testOrExam = _testsAndExamsRepository.FindById(testOrExamId);
            var courseId = testOrExam.CourseId;
            if (testOrExam != null)
            {
                var deleteSuccess = _testsAndExamsRepository.Delete(testOrExam);
                if (deleteSuccess)
                {
                    return RedirectToAction("ViewCourseTestsAndExams", new { courseId = courseId });
                }
                return View("Error", "Home");
            }
            return NotFound();
        }

    }
}