using AcademyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.Contracts
{
    public interface ITestsAndExamsRepository : IBaseRepository<TestsAndExams>
    {
        ICollection<TestsAndExams> GetTestsAndExamsByCourseId(int courseId);
    }
}
