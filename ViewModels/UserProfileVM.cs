using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.ViewModels
{
    public class UserProfileVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Display(Name = "First name:")]
        public string FirstName { get; set; }
        [Display(Name = "Last name:")]
        public string LastName { get; set; }
        [Display(Name = "Middle name:")]
        public string MiddleName { get; set; }
        [Display(Name = "Phone:")]
        public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        public string StudentId { get; set; }
    }

    public class ChangePasswordVM
    {
        public string Id { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
        [Required]
        [DataType(DataType.Password, ErrorMessage = "Wrong password format")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password, ErrorMessage = "Wrong password format")]
        [Display(Name = "Confirm New Password")]
        public string ConfiremNewPassword { get; set; }
    }

    public class EditProfileVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Display(Name = "First name:")]
        public string FirstName { get; set; }
        [Display(Name = "Last name:")]
        public string LastName { get; set; }
        [Display(Name = "Middle name:")]
        public string MiddleName { get; set; }
        [Display(Name = "Phone:")]
        [Phone]
        public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        //public IFormFile Picture { get; set; }
    }
}
