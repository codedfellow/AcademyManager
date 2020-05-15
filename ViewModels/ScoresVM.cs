using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.ViewModels
{
    public class ScoresVM
    {
        public int Id { get; set; }
        public TraineeVM Trainee { get; set; }
        public string TraineeId { get; set; }
        public TestAndExamVM TestOrExam { get; set; }
        public int TestOrExamId { get; set; }
        public double Score { get; set; }
    }
}
