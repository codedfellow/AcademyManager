using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.ViewModels
{
    public class TotalPoints
    {
        public string TraineeId { get; set; }
        public double TotalPoint { get; set; }
    }

    public class TotalPointsViewModel
    {
        public int CourseId { get; set; }
        public List<TotalPoints> Points { get; set; }
    }
}
