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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademyManager.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AMUser> _userManager;

        public CoursesController(ICoursesRepository coursesRepository, IMapper mapper, UserManager<AMUser> userManager)
        {
            _coursesRepository = coursesRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var courses = _coursesRepository.FindAll().ToList();
            var model = _mapper.Map<List<CourseVM>>(courses);
            return View(model);
        }

        public IActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCourse(CreateCourseVM model)
        {
            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Courses>(model);
                var isSuccess = _coursesRepository.Create(course);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "An error occured while creating the courses");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("", "Please fill all the fields correctly");
            return View(model);
        }

        public async Task<IActionResult> EditCourse(int id)
        {
            var course = _coursesRepository.FindById(id);
            var facilitators = await _userManager.GetUsersInRoleAsync("Facilitator");
            var selectFacilitators = facilitators.Select(p => new SelectListItem
            {
                Text = $"{p.FirstName} {p.MiddleName} {p.LastName}",
                Value = p.Id
            });

            var model = new EditCourseVM
            {
                Id = course.Id,
                CourseName = course.CourseName,
                Facilitators = selectFacilitators
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCourse(EditCourseVM model)
        {
            try
            {
                var course = _coursesRepository.FindById(model.Id);
                
                if (!ModelState.IsValid)
                {
                    var facilitators = await _userManager.GetUsersInRoleAsync("Facilitator");
                    var selectFacilitators = facilitators.Select(p => new SelectListItem
                    {
                        Text = $"{p.FirstName} {p.MiddleName} {p.LastName}",
                        Value = p.Id
                    });
                    model = new EditCourseVM
                    {
                        Id = course.Id,
                        CourseName = course.CourseName,
                        Facilitators = selectFacilitators
                    };
                    model.Facilitators = selectFacilitators;
                    ModelState.AddModelError("", "Please fill all the required fields correctly");
                    return View(model);
                }
                course.FacilitatorId = model.FacilitatorId;
                //var courseUpdateModel = new CourseVM
                //{
                //    Id = model.Id,
                //    CourseName = model.CourseName,
                //    FacilitatorId = model.FacilitatorId
                //};
                //var courseUpdate = _mapper.Map<Courses>(courseUpdateModel);
                var isSuccess = _coursesRepository.Update(course);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "An error occured while updating the course");
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"An error occured: {e}");
                return View(model);
            }
        }
    }
}