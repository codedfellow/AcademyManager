using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyManager.ViewModels;

namespace AcademyManager.Models
{
    public class AppDbContext : IdentityDbContext<AMUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<TestsAndExams> TestsAndExams { get; set; }
        public DbSet<Scores> Scores { get; set; }
        public DbSet<TraineeSerialStore> TraineeSerialStores { get; set; }
        public DbSet<AcademyManager.ViewModels.CreateCourseVM> CreateCourseVM { get; set; }
        public DbSet<AcademyManager.ViewModels.CourseVM> CourseVM { get; set; }
        public DbSet<AcademyManager.ViewModels.TestAndExamVM> TestAndExamVM { get; set; }
        public DbSet<AcademyManager.ViewModels.TraineeVM> TraineeVM { get; set; }
        public DbSet<AcademyManager.ViewModels.ScoresVM> ScoresVM { get; set; }
        public DbSet<AcademyManager.ViewModels.FacilitatorVM> FacilitatorVM { get; set; }
        public DbSet<AcademyManager.ViewModels.UserVM> UserVM { get; set; }
    }
}
