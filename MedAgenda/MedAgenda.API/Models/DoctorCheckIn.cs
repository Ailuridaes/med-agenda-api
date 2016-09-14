using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedAgenda.API.Models
{
    public class DoctorCheckIn
    {
        public int DoctorCheckInId { get; set; }
        public int DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}