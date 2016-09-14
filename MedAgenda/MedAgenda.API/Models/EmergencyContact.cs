using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedAgenda.API.Models
{
    public class EmergencyContact
    {
        public int EmergencyContactID { get; set; }
        public int PatientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }

        public virtual Patient Patient { get; set; }

    }
}