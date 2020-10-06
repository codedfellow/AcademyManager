using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AcademyManager.Models;
using AutoMapper;
using AcademyManager.Mappings;
using AcademyManager.Repository;
using AcademyManager.Contracts;

namespace AcademyManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IScoresRepository, ScoresRepository>();
            services.AddScoped<ICoursesRepository, CoursesRepository>();
            services.AddScoped<ITestsAndExamsRepository, TestsAndExamsRepository>();
            services.AddScoped<ISerialStoreRepository, SerialStoreRepository>();

            services.AddAutoMapper(typeof(Maps));
            services.AddIdentity<AMUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<AppDbContext>();
            services.AddControllersWithViews();
            services.AddAuthorization(option =>
            {
                option.AddPolicy("RootAdminPolicy", policy => policy.RequireUserName("admin@localhost.com"));
            });
            services.AddRazorPages();

            //services.Configure<IISServerOptions>(options =>
            //{
            //    options.AutomaticAuthentication = false;
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /* 
         This memthod injects usermanager and role manager so that whenever the application is ran, the class that checks if
         the default user and all application roles already exist is executed and the default user and roles are created if they
         do not exist when the app is ran for the first time
             */
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<AMUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // The default user and roles creating class is called here
            DefaultUserssAndRoles.CreateDefaultUsersAndRoles(userManager, roleManager);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Welcome}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
