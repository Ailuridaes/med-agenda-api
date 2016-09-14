using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedAgenda.API.Models
{
    public class MedicalField
    {
        public int MedicalFieldId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PatientCheckIn> PatientCheckIns { get; set; }
        public virtual ICollection<ExamRoomPurpose> ExamRoomPurposes { get; set; }
        public virtual ICollection<Specialty> Specialties { get; set; }
    }
}