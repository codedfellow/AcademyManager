using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyManager.Contracts;
using AcademyManager.Models;
using AcademyManager.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademyManager.Controllers
{
    public class TraineesController : Controller
    {
        private readonly UserManager<AMUser> _userManager;
        private readonly SignInManager<AMUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ISerialStoreRepository _serialStore;

        public TraineesController(UserManager<AMUser> userManager, SignInManager<AMUser> signInManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper, ISerialStoreRepository serialStore)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _serialStore = serialStore;
        }
        // GET: Students
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Trainee");
            var model = _mapper.Map<IList<AMUser>, IList<TraineesVM>>(users);
            return View(model);
        }

        // GET: Students/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Students/Create
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
            var model = _mapper.Map<List<AMUser>,List<TraineesVM>>(toBeSelected);
            return View(model);
        }

        // POST: Students/Create
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
                return RedirectToAction("Index","AdminPortal");
            }
            else
            {
                ModelState.AddModelError("", "An error occured");
                return View(model);
            }
        }

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

        public async Task<IActionResult> AssignTraineeSerial()
        {
            var trainees = await _userManager.GetUsersInRoleAsync("Trainee");
            var toBeAssigned = trainees.Where(p => p.TraineeId == null).ToList();
            var model = _mapper.Map<List<TraineesVM>>(toBeAssigned);
            return View(model);
        }
        // GET: Students/Edit/5
        
        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Students/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}