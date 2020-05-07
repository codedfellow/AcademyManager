using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.Models
{
    public class Courses
    {
        [Key]
        public int Id { get; set; }
        public string CourseName { get; set; }
        [ForeignKey("FacilitatorId")]
        public AMUser Facilitator { get; set; }
        public string FacilitatorId { get; set; }
    }
}
