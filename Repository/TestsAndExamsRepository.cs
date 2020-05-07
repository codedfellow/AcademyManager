using AcademyManager.Contracts;
using AcademyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.Repository
{
    public class TestsAndExamsRepository : ITestsAndExamsRepository
    {
        private readonly AppDbContext _db;

        public TestsAndExamsRepository(AppDbContext db)
        {
            _db = db;
        }
        public bool Create(TestsAndExams entity)
        {
            _db.TestsAndExams.Add(entity);
            return Save();
        }

        public bool Delete(TestsAndExams entity)
        {
            _db.TestsAndExams.Remove(entity);
            return Save();
        }

        public ICollection<TestsAndExams> FindAll()
        {
            var testsAndExams = _db.TestsAndExams.ToList();
            return testsAndExams;
        }

        public TestsAndExams FindById(int id)
        {
            var testOrExam = _db.TestsAndExams.Find(id);
            return testOrExam;
        }

        public ICollection<TestsAndExams> GetTestsAndExamsByCourseId(int courseId)
        {
            var courseTestsAndExams = FindAll().Where(p => p.CourseId == courseId).ToList();
            return courseTestsAndExams;
        }

        public bool IsExist(int id)
        {
            var exists = _db.TestsAndExams.Any(q => q.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(TestsAndExams entity)
        {
            _db.TestsAndExams.Update(entity);
            return Save();
        }
    }
}
