﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.ViewModels
{
    public class GeneralCourseDetailsVM
    {
        public int CourseId { get; set; }
        public List<CurrentTestOrExamVM> TestsOrExams { get; set; }
    }

    public class CurrentTestOrExamVM
    {
        public int TestOrExamId { get; set; }
        public List<ScoresVM> Scores { get; set; }
    }
}
