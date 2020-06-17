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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademyManager.Controllers
{
    // This controller is used by adminitrators to manage the app
    [Authorize(Roles = "Administrator")]
    public class AdministratorsController : Controller
    {
        // Dependency injection and initialization in the constructor
        private readonly UserManager<AMUser> _userManager;
        private readonly SignInManager<AMUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ISerialStoreRepository _serialStore;
        private readonly ICoursesRepository _coursesRepository;

        public AdministratorsController(UserManager<AMUser> userManager, SignInManager<AMUser> signInManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper, ISerialStoreRepository serialStore, ICoursesRepository coursesRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _serialStore = serialStore;
            _coursesRepository = coursesRepository;
        }

        // This index view displays the list of administrators
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Administrator");
            var model = _mapper.Map<IList<AMUser>, IList<FacilitatorsVM>>(users);
            return View(model);
        }

        // This get method returns the view that is used to create app administrators
        [HttpGet]
        public async Task<IActionResult> CreateAdministrators(int id)
        {
            var appUsers = _userManager.Users.AsEnumerable().ToList();

            List<AMUser> toBeSelected = new List<AMUser>();
            for (int i = 0; i < appUsers.Count; i++)
            {
                if (!(await _userManager.IsInRoleAsync(appUsers[i], "Trainee"))
                    && !(await _userManager.IsInRoleAsync(appUsers[i], "Administrator")))
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


        // This method handles the post method for creating facilitators
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdministrators(List<FacilitatorsVM> model)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    if (model[i].IsSelected)
                    {
                        var user = await _userManager.FindByIdAsync(model[i].Id);
                        var result = await _userManager.AddToRoleAsync(user, "Administrator");
                        if (result.Succeeded)
                        {
                            continue;
                        }
                        else if (!(result.Succeeded))
                        {
                            ModelState.AddModelError("", "An error occured while creating the administrators");
                            return View(model);
                        }
                    }
                    else
                        continue;
                }
                return RedirectToAction("Index", "Administrators");
            }
            else
            {
                ModelState.AddModelError("", "An error occured");
                return View(model);
            }
        }

        // This method is the get view that is used to create facilitators
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


        // This method handles the post method of the create facilitators view
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
                return RedirectToAction("FacilitatorsList");
            }
            else
            {
                ModelState.AddModelError("", "An error occured");
                return View(model);
            }
        }

        // This method is the get method that is used to create trainees
        [HttpGet]
        public async Task<IActionResult> CreateTrainees()
        {
            var appUsers = _userManager.Users.AsEnumerable().ToList();
            List<AMUser> toBeSelected = new List<AMUser>();
            for (int i = 0; i < appUsers.Count; i++)
            {
                if (!(await _userManager.IsInRoleAsync(appUsers[i], "Administrator"))
                    && !(await _userManager.IsInRoleAsync(appUsers[i], "Trainee"))
                    && !(await _userManager.IsInRoleAsync(appUsers[i], "Facilitator")))
                {
                    toBeSelected.Add(appUsers[i]);
                    continue;
                }
                else
                    continue;
            }
            var model = _mapper.Map<List<AMUser>, List<TraineesVM>>(toBeSelected);
            return View(model);
        }

        // This method handles the post action of the create trainees form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainees(List<TraineesVM> model)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    if (model[i].IsSelected)
                    {
                        var user = await _userManager.FindByIdAsync(model[i].Id);
                        var result = await _userManager.AddToRoleAsync(user, "Trainee");
                        if (result.Succeeded)
                        {
                            continue;
                        }
                        else if (!(result.Succeeded))
                        {
                            ModelState.AddModelError("", "An error occured while creating the students");
                            return View(model);
                        }
                    }
                    else
                        continue;
                }
                return RedirectToAction("TraineesList");
            }
            else
            {
                ModelState.AddModelError("", "An error occured");
                return View(model);
            }
        }

        // This method is used to assign serial number to a trainee
        public async Task<IActionResult> AssignTraineeSerial()
        {
            var trainees = await _userManager.GetUsersInRoleAsync("Trainee");
            var toBeAssigned = trainees.Where(p => p.TraineeId == null).ToList();
            var model = _mapper.Map<List<TraineesVM>>(toBeAssigned);
            return View(model);
        }


        // THis method is used to generate the serial number of a trainee
        public async Task<IActionResult> GenerateSerial(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            var serial = _serialStore.FindById(1);
            int serialNum = serial.Serial;
            var serialCount = serialNum + 1;
            var fullSerial = "STD" + serialCount.ToString();
            user.TraineeId = fullSerial;
            serial.Serial = serialCount;
            _serialStore.Update(serial);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("AssignTraineeSerial");
            }
            return View("Error");
        }

        // THis method returns the view of the list of trainees
        public IActionResult TraineesList()
        {
            var users = _userManager.GetUsersInRoleAsync("Trainee").Result;
            var model = _mapper.Map<IList<AMUser>, IList<TraineeVM>>(users);
            return View(model);
        }

        // This method reutrns the view of the list of facilitators
        public IActionResult FacilitatorsList()
        {
            var users = _userManager.GetUsersInRoleAsync("Facilitator").Result;
            var model = _mapper.Map<IList<AMUser>, IList<FacilitatorVM>>(users);
            return View(model);
        }

        // This method returns the get view for creating a course
        public IActionResult CreateCourse()
        {
            return View();
        }

        // This method is for the post action that creates a course
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
                    return RedirectToAction("CourseList");
                }
            }
            ModelState.AddModelError("", "Please fill all the fields correctly");
            return View(model);
        }

        // This method returns the get view for creating courses
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

        // This method edits an alaready exisiting course
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
                if (model.FacilitatorId != "Select Facilitator")
                {
                    course.FacilitatorId = model.FacilitatorId;
                }
                else
                {
                    course.FacilitatorId = null;
                }
                
                course.CourseName = model.CourseName;
                var isSuccess = _coursesRepository.Update(course);
                if (!isSuccess)
                {
                    ModelState.AddModelError("", "An error occured while updating the course");
                    return View(model);
                }
                return RedirectToAction("CourseList");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", $"An error occured: {e}");
                return View(model);
            }
        }

        // This method removes a user from the trainee role
        public IActionResult RemoveFromTraineeRole(string id)
        {
            var trainee = _userManager.FindByIdAsync(id).Result;
            var result = _userManager.RemoveFromRoleAsync(trainee, "Trainee").Result;
            if (result.Succeeded)
            {
                trainee.TraineeId = null;
                var isUpdated = _userManager.UpdateAsync(trainee).Result;
                if (isUpdated.Succeeded)
                {
                    return RedirectToAction("TraineesList");
                }
            }
            return View("Error", "Home");
        }

        // This method removes a user from the facilitator role
        public IActionResult RemoveFromFacilitatorRole(string id)
        {
            var facilitator = _userManager.FindByIdAsync(id).Result;
            var result = _userManager.RemoveFromRoleAsync(facilitator, "Facilitator").Result;
            if (result.Succeeded)
            {
                var facilitatorCourse = _coursesRepository.GetCourseByFacilitatorId(id);
                foreach (var item in facilitatorCourse)
                {
                    item.FacilitatorId = null;
                    _coursesRepository.Update(item);
                }
                return RedirectToAction("FacilitatorsList");
            }
            return View("Error", "Home");
        }

        // This method removes a user from the admin role
        public IActionResult RemoveFromAdminRole(string id)
        {
            var admin = _userManager.FindByIdAsync(id).Result;
            var result = _userManager.RemoveFromRoleAsync(admin, "Administrator").Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View("Error", "Home");
        }

        // This method returns a view containing the list of courses
        public IActionResult CourseList()
        {
            var courses = _coursesRepository.FindAll().ToList();
            var model = _mapper.Map<List<CourseVM>>(courses);
            return View(model);
        }

        // THis method is used to delete a course
        public IActionResult DeleteCourse(int id)
        {
            var course = _coursesRepository.FindById(id);
            if (course != null)
            {
                var deleted = _coursesRepository.Delete(course);
                if (deleted)
                {
                    return RedirectToAction("CourseList");
                }
                return View("Error", "Home");
            }
            return View("Error", "Home");
        }

        // Thus method returns a view containing the list of all the app users
        public IActionResult AllUsersList()
        {
            var allUsers = _userManager.Users.ToList();
            var model = _mapper.Map<List<UserVM>>(allUsers);
            return View(model);
        }

        //public IActionResult ResultReady()
        //{
        //    return View();
        //}
    }
}
