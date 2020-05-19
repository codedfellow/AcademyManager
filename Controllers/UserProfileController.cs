using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AcademyManager.Models;
using AcademyManager.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcademyManager.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IMapper mapper;
        private readonly UserManager<AMUser> userManager;
        private readonly SignInManager<AMUser> signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserProfileController(IMapper mapper, UserManager<AMUser> userManager, SignInManager<AMUser> signInManager,
            IWebHostEnvironment hostEnvironment)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            if (!signInManager.IsSignedIn(User))
            {
                return View("Error");
            }
            else
            {
                var user = User.Identity.Name;
                var signedInUser = await userManager.FindByNameAsync(user);
                var model = mapper.Map<UserProfileVM>(signedInUser);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string id)
        {
            var signedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!(signedInUser == id))
            {
                return View("Error");
            }
            else
            {
                var userToEdit = await userManager.FindByIdAsync(signedInUser);
                var model = mapper.Map<EditProfileVM>(userToEdit);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill out the form correctly");
                return View(model);
            }
            else
            {
                var editedUser = await userManager.FindByIdAsync(model.Id);
                if (editedUser == null)
                {
                    return NotFound();
                }
                else
                {
                    editedUser.Id = model.Id;
                    editedUser.Email = model.Email;
                    editedUser.FirstName = model.FirstName;
                    editedUser.LastName = model.LastName;
                    editedUser.MiddleName = model.MiddleName;
                    editedUser.PhoneNumber = model.PhoneNumber;
                    editedUser.UserName = model.Email;
                    if (model.Picture != null)
                    {
                        if (model.PictureUrl != null)
                        {
                            string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", model.PictureUrl);
                            System.IO.File.Delete(filePath);
                        }
                        editedUser.PictureUrl = ProcessUploadedFile(model);
                    }
                    else
                    {
                        if (model.PictureUrl != null)
                        {
                            string filePath = Path.Combine(_hostEnvironment.WebRootPath, "images", model.PictureUrl);
                            System.IO.File.Delete(filePath);
                        }
                        editedUser.PictureUrl = null;
                    }
                    var result = await userManager.UpdateAsync(editedUser);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
        }
        private string ProcessUploadedFile(EditProfileVM model)
        {
            string uniqueFileName = null;
            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath + "/images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Picture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.Picture.CopyTo(fileStream);
            }
            else
            {
                return uniqueFileName;
            }

            return uniqueFileName;
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var model = new ChangePasswordVM
            {
                Id = user.Id
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please fill all fields");
                return View(model);
            }
            else
            {
                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Please enter the correct current password and the new password must contain " +
                        "an uppercase letter, lower case letters, a number and a special character");
                    return View(model);
                }
                return RedirectToAction("index");
            }
        }
    }
}
