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
        public bool Create(AppStateStore entity)
        {
            _db.AppStateStore.Add(entity);
            return Save();
        }

        public bool Delete(AppStateStore entity)
        {
            _db.AppStateStore.Remove(entity);
            return Save();
        }

        public ICollection<AppStateStore> FindAll()
        {
            var serials = _db.AppStateStore.ToList();
            return serials;
        }

        public AppStateStore FindById(int id)
        {
            var serial = _db.AppStateStore.Find(id);
            return serial;
        }

        public bool IsExist(int id)
        {
            var exists = _db.AppStateStore.Any(q => q.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(AppStateStore entity)
        {
            _db.AppStateStore.Update(entity);
            return Save();
        }
    }
}
