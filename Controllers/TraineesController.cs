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
        private readonly IMapper _mapper;
        private readonly ICoursesRepository _coursesRepository;
        private readonly IScoresRepository _scoresRepository;
        private readonly ITestsAndExamsRepository _testsAndExamsRepository;

        public TraineesController(UserManager<AMUser> userManager,
            IMapper mapper, ICoursesRepository coursesRepository,
            IScoresRepository scoresRepository, ITestsAndExamsRepository testsAndExamsRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
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
            var scoresList = new List<CurrentTestOrExamVM>();
            if (courseTestsAndExams.Count > 0)
            {
                for (int i = 0; i < courseTestsAndExams.Count; i++)
                {
                    var scores = _scoresRepository.GetScoreByTestOrExamId(courseTestsAndExams[i].Id).ToList();
                    if (scores.Count > 0 )
                    {
                        var scoreModel = _mapper.Map<List<ScoresVM>>(scores);
                        var selected = new CurrentTestOrExamVM
                        {
                            TestOrExamId = courseTestsAndExams[i].Id,
                            Scores = scoreModel
                        };
                        scoresList.Add(selected);
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
                TestsOrExams = scoresList
            };
            return View(model);
        }

        public IActionResult PersonalCourseDetails(int courseId)
        {
            var courseTestsOrExams = _testsAndExamsRepository.GetTestsAndExamsByCourseId(courseId).ToList();
            var scores = new List<ScoresVM>();
            var trainee = _userManager.GetUserAsync(User).Result;
            if (courseTestsOrExams.Count > 0)
            {
                for (int i = 0; i < courseTestsOrExams.Count; i++)
                {
                    var score = _scoresRepository.GetScoreByTestAndExamIdAndTraineeId(courseTestsOrExams[i].Id, trainee.Id);
                    var scoreModel = _mapper.Map<ScoresVM>(score);
                    scores.Add(scoreModel);
                }
            }
            var model = new PersonalCourseDetailsVM
            {
                CourseId = courseId,
                Scores = scores
            };
            return View(model);
        }
    }
}