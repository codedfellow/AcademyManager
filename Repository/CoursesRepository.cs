using AcademyManager.Contracts;
using AcademyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.Repository
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly AppDbContext _db;

        public CoursesRepository(AppDbContext db)
        {
            _db = db;
        }
        public bool Create(Courses entity)
        {
            _db.Courses.Add(entity);
            return Save();
        }

        public bool Delete(Courses entity)
        {
            _db.Courses.Remove(entity);
            return Save();
        }

        public ICollection<Courses> FindAll()
        {
            var courses = _db.Courses.ToList();
            return courses;
        }

        public Courses FindById(int id)
        {
            var course = _db.Courses.Find(id);
            return course;
        }

        public ICollection<Courses> GetCourseByFacilitatorId(string id)
        {
            var courses = FindAll().Where(p => p.FacilitatorId == id).ToList();
            return courses;
        }

        public bool IsExist(int id)
        {
            var exists = _db.Courses.Any(q => q.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(Courses entity)
        {
            _db.Courses.Update(entity);
            return Save();
        }
    }
}
