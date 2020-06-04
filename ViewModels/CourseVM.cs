using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.ViewModels
{
    public class CourseVM
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public FacilitatorVM Facilitator { get; set; }
        public string FacilitatorId { get; set; }
    }

    public class CreateCourseVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Add a valid course name")]
        [DataType(DataType.Text)]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
    }

    public class EditCourseVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Add a valid course name")]
        [DataType(DataType.Text)]
        public string CourseName { get; set; }
        public IEnumerable<SelectListItem> Facilitators { get; set; }
        public string FacilitatorId { get; set; }
    }
}
