using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.ViewModels
{
    public class TraineesVM
    {
        public string Id { get; set; }
        public string TraineeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsSelected { get; set; }
    }

    public class TraineeVM
    {
        public string Id { get; set; }
        public string TraineeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }

}
