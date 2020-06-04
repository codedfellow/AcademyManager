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
    //This controller manages user registration and login
    public class AccountController : Controller
    {
        //Dependency injection and initialization
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
        //This action handles get method of the register view
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //This action manages the post method of the user registration form
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Map the view model to the AMUser class
                var user = mapper.Map<AMUser>(model);
                user.UserName = model.Email;

                // create the new user
                var result = await userManager.CreateAsync(user, model.Password);

                /* 
                 If user is successfully created login and navigate the user to the application home page
                 */
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                // Check for errors in the user form entry and display them to the user
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        //THis action handles the get method of the login view
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // This method handles the post submission of the login form
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Sign in the user if the credentials are valid
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("index", "Home");
                }

                // Check for errors in the users input and display them to the user
                ModelState.AddModelError("", "Invalid username or password");
            }
            return View(model);
        }

        // This function logs the user out of the application
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Welcome");
        }

        // This method checks if the email the new user wants to use for registration is already in use
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

        // This method displays the access denied view when a user tries to view any resource he/she is not permitted to view
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}