using AcademyManager.Models;
using AcademyManager.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<AMUser, RegisterViewModel>().ReverseMap();
            CreateMap<AMUser, UserProfileVM>().ReverseMap();
            CreateMap<AMUser, EditProfileVM>().ReverseMap();
            CreateMap<AMUser, TraineesVM>().ReverseMap();
            CreateMap<AMUser, FacilitatorsVM>().ReverseMap();
            CreateMap<Courses, CourseVM>().ReverseMap();
            CreateMap<Courses, CreateCourseVM>().ReverseMap();
            CreateMap<Courses, EditCourseVM>().ReverseMap();
            CreateMap<TestsAndExams, TestAndExamVM>().ReverseMap();
        }
    }
}
