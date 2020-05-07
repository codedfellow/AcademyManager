using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.Models
{
    public class Scores
    {
        public int Id { get; set; }
        [ForeignKey("TraineeId")]
        public AMUser Trainee { get; set; }
        public string TraineeId { get; set; }
        [ForeignKey("TestOrExamId")]
        public TestsAndExams TestOrExam { get; set; }
        public int TestOrExamId { get; set; }
        public double Score { get; set; }
    }
}
