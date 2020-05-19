using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.ViewModels
{
    public class PersonalCourseDetailsVM
    {
        public int CourseId { get; set; }
        public List<ScoresVM> Scores { get; set; }
    }
}
