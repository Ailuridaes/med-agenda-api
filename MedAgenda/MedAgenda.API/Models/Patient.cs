using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MedAgenda.API.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Boolean IsDisabled { get; set; }
        public string Medications { get; set; }
        public string ChronicConditions { get; set; }
        public string Allergies { get; set; }
        public string Symptoms { get; set; }

        public virtual ICollection<PatientCheckIn> PatientCheckIns { get; set; }
        public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; }

    }
}