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
                    if (score != null)
                    {
                        var scoreModel = _mapper.Map<ScoresVM>(score);
                        scores.Add(scoreModel);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            var model = new PersonalCourseDetailsVM
            {
                CourseId = courseId,
                Scores = scores
            };
            return View(model);
        }

        [AllowAnonymous]
        [Authorize(Roles = "Trainee, Facilitator")]
        public IActionResult TotalPoints(int courseId)
        {
            var testsAndExams = _testsAndExamsRepository.GetTestsAndExamsByCourseId(courseId).ToList();
            var allScores = new List<List<Scores>>();
            int courseTotal = 0;
            foreach (var item in testsAndExams)
            {
                var scores = _scoresRepository.GetScoreByTestOrExamId(item.Id).ToList();
                courseTotal += item.Total;
                allScores.Add(scores);
            }
            
            var trainees = new List<string>();
            foreach (var item in allScores)
            {
                foreach (var scoreSet in item)
                {
                    if (!trainees.Contains(scoreSet.TraineeId))
                    {
                        trainees.Add(scoreSet.TraineeId);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            var traineePoints = new List<TotalPoints>();
            foreach (var item in trainees)
            {
                double total = 0;
                foreach (var scoreSet in testsAndExams)
                {
                    var score = _scoresRepository.GetScoreByTestAndExamIdAndTraineeId(scoreSet.Id, item);
                    if (score == null)
                    {
                        total += 0;
                    }
                    else
                    {
                        total += score.Score;
                    }
                }
                var average = (total / courseTotal) * 100;
                var totalTraineePoint = new TotalPoints
                {
                    TraineeId = item,
                    TotalPoint = Math.Round(average, 2)
                };
                traineePoints.Add(totalTraineePoint);
            }
            var model = new TotalPointsViewModel
            {
                CourseId = courseId,
                Points = traineePoints
            };
            return View(model);
        }

        public IActionResult Result()
        {
            var trainee = _userManager.GetUserAsync(User).Result;
            var traineeScores = _scoresRepository.GetScoresByTraineeId(trainee.Id).ToList();
            var traineeCourses = new List<int>();

            foreach (var item in traineeScores)
            {
                var testOrExam = _testsAndExamsRepository.FindById(item.TestOrExamId);
                var course = _coursesRepository.FindById(testOrExam.CourseId);
                if (traineeCourses.Contains(course.Id))
                {
                    continue;
                }
                else
                {
                    traineeCourses.Add(course.Id);
                }
            }

            var traineeResult = new List<TotalCourseScoreVM>();

            foreach (var item in traineeCourses)
            {
                double totalScore = 0;
                var totalMark = 0;
                var testOrExams = _testsAndExamsRepository.GetTestsAndExamsByCourseId(item).ToList();
                for (int i = 0; i < testOrExams.Count; i++)
                {
                    totalMark += testOrExams[i].Total;
                    var score = _scoresRepository.GetScoreByTestAndExamIdAndTraineeId(testOrExams[i].Id, trainee.Id);
                    if (score != null)
                    {
                        totalScore += score.Score;
                    }
                }

                var averageScore = (totalScore / totalMark) * 100;
                var courseScore = new TotalCourseScoreVM
                {
                    CoureId = item,
                    TotalScore = Math.Round(averageScore, 2)
                };
                traineeResult.Add(courseScore);
            }
            var model = traineeResult;
            return View(model);
        }
    }
}