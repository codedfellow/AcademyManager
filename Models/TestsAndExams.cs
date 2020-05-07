using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.Models
{
    public class TestsAndExams
    {
        [Key]
        public int Id { get; set; }
        public string TestOrExamName { get; set; }
        [ForeignKey("CourseId")]
        public Courses Course { get; set; }
        public int CourseId { get; set; }
        public int Total { get; set; }
    }
}
