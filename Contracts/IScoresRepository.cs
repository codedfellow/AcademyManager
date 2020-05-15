using AcademyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.Contracts
{
    public interface IScoresRepository : IBaseRepository<Scores>
    {
        Scores GetScoreByTestAndExamIdAndTraineeId(int testOrExamId, string traineeId);
        ICollection<Scores> GetScoreByTestOrExamId(int testOrExamId);
    }
}
