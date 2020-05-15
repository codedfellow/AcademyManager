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
    }
}