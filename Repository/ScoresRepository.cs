﻿using AcademyManager.Contracts;
using AcademyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.Repository
{
    public class ScoresRepository : IScoresRepository
    {
        private readonly AppDbContext _db;

        public ScoresRepository(AppDbContext db)
        {
            _db = db;
        }
        public bool Create(Scores entity)
        {
            _db.Scores.Add(entity);
            return Save();
        }

        public bool Delete(Scores entity)
        {
            _db.Scores.Remove(entity);
            return Save();
        }

        public ICollection<Scores> FindAll()
        {
            var scores = _db.Scores.ToList();
            return scores;
        }

        public Scores FindById(int id)
        {
            var score = _db.Scores.Find(id);
            return score;
        }

        public bool IsExist(int id)
        {
            var exists = _db.Scores.Any(q => q.Id == id);
            return exists;
        }

        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool Update(Scores entity)
        {
            _db.Scores.Update(entity);
            return Save();
        }
    }
}