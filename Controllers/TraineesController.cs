using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyManager.Contracts;
using AcademyManager.Models;
using AcademyManager.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademyManager.Controllers
{
    [Authorize(Roles = "Trainee")]
    public class TraineesController : Controller
    {
        private readonly UserManager<AMUser> _userManager;
        private readonly SignInManager<AMUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ISerialStoreRepository _serialStore;
        private readonly ICoursesRepository _coursesRepository;
        private readonly IScoresRepository _scoresRepository;
        private readonly ITestsAndExamsRepository _testsAndExamsRepository;

        public TraineesController(UserManager<AMUser> userManager, SignInManager<AMUser> signInManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper, ISerialStoreRepository serialStore, ICoursesRepository coursesRepository,
            IScoresRepository scoresRepository, ITestsAndExamsRepository testsAndExamsRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _serialStore = serialStore;
            _coursesRepository = coursesRepository;
            _scoresRepository = scoresRepository;
            _testsAndExamsRepository = testsAndExamsRepository;
        }
        // GET: Students
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Trainee");
            var model = _mapper.Map<IList<AMUser>, IList<TraineesVM>>(users);
            return View(model);
        }

        public IActionResult CourseList()
        {
            var courses = _coursesRepository.FindAll().ToList();
            var model = _mapper.Map<List<CourseVM>>(courses);
            return View(model);
        }

        public IActionResult GeneralCourseDetails(int courseId)
        {
            var courseTestsAndExams = _testsAndExamsRepository.GetTestsAndExamsByCourseId(courseId).ToList();
            var scoresList = new List<List<ScoresVM>>();
            if (courseTestsAndExams.Count > 0)
            {
                for (int i = 0; i < courseTestsAndExams.Count; i++)
                {
                    var scores = _scoresRepository.GetScoreByTestOrExamId(courseTestsAndExams[i].Id).ToList();
                    if (scores.Count > 0 )
                    {
                        var scoreModel = _mapper.Map<List<ScoresVM>>(scores);
                        scoresList.Add(scoreModel);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            var model = new GeneralCourseDetailsVM
            {
                CourseId = courseId,
                Scores = scoresList
            };
            return View(model);
        }
    }
}