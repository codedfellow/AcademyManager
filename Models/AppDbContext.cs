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
        public DbSet<AppStateStore> AppStateStore { get; set; }
    }
}
