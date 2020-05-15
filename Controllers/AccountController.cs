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
    public class AccountController : Controller
    {
        private readonly UserManager<AMUser> userManager;
        private readonly SignInManager<AMUser> signInManager;
        private readonly IMapper mapper;

        public AccountController(UserManager<AMUser> userManager, SignInManager<AMUser> signInManager,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }
        //This action handles the register view
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var user = new AMUser { UserName = model.Email, Email = model.Email };
                var user = mapper.Map<AMUser>(model);
                user.UserName = model.Email;
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        //THis action handles the login view
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "Home");
                }
                ModelState.AddModelError("", "Invalid username or password");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("welcome", "home");
        }

        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}