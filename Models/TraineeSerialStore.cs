using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyManager.Models
{
    public class TraineeSerialStore
    {
        [Key]
        public int Id { get; set; }
        public int Serial { get; set; }
    }
}
