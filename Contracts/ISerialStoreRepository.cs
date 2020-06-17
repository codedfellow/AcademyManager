using AcademyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.Contracts
{
    public interface ISerialStoreRepository : IBaseRepository<AppStateStore>
    {
    }
}
