using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyManager.Models;
using AcademyManager.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademyManager.Controllers
{
    public class AdministratorsController : Controller
    {
        private readonly UserManager<AMUser> _userManager;
        private readonly SignInManager<AMUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AdministratorsController(UserManager<AMUser> userManager, SignInManager<AMUser> signInManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Administrator");
            var model = _mapper.Map<IList<AMUser>, IList<FacilitatorsVM>>(users);
            return View(model);
        }

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
                return RedirectToAction("Index", "AdminPortal");
            }
            else
            {
                ModelState.AddModelError("", "An error occured");
                return View(model);
            }
        }


    }
}
