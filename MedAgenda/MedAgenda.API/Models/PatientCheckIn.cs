using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedAgenda.API.Models
{
    public class PatientCheckIn
    {
        public int PatientCheckInId { get; set; }
        public int? MedicalFieldId { get; set; }
        public int PatientId { get; set; }
        public int PainScale { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual MedicalField MedicalField { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}