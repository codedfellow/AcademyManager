using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.ViewModels
{
    public class TestAndExamVM
    {
        public int Id { get; set; }
        [Display(Name = "Test or Exam Name")]
        public string TestOrExamName { get; set; }
        public CourseVM Course { get; set; }
        public int CourseId { get; set; }
        public int Total { get; set; }
    }

    public class ViewCourseTestsAndExamsVM
    {
        public int CourseId { get; set; }
        public List<TestAndExamVM> TestsAndExams { get; set; }
    }

    public class ListTraineesForTestVM
    {
        public TestAndExamVM TestOrExam { get; set; }
        public List<TraineeVM> TraineesForTest { get; set; }
    }
}
