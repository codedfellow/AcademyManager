using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.ViewModels
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Email")]
        [Remote(action: "IsEmailInUse", controller:"Account")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage = "The password fields must match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
