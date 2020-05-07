using AcademyManager.Contracts;
using AcademyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.Repository
{
    public class SerialStoreRepository : ISerialStoreRepository
    {
        private readonly AppDbContext _db;

        public SerialStoreRepository(AppDbContext db)
        {
            _db = db;
        }
        public bool Create(TraineeSerialStore entity)
        {
            _db.TraineeSerialStores.Add(entity);
            return Save();
        }

        public bool Delete(TraineeSerialStore entity)
        {
            _db.TraineeSerialStores.Remove(entity);
            return Save();
        }

        public ICollection<TraineeSerialStore> FindAll()
        {
            var serials = _db.TraineeSerialStores.ToList();
            return serials;
        }

        public TraineeSerialStore FindById(int id)
        {
            var serial = _db.TraineeSerialStores.Find(id);
            return serial;
        }

        public bool IsExist(int id)
        {
            var exists = _db.TraineeSerialStores.Any(q => q.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(TraineeSerialStore entity)
        {
            _db.TraineeSerialStores.Update(entity);
            return Save();
        }
    }
}
