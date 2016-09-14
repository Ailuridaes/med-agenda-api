namespace MedAgenda.API.Models
{
    public class Specialty
    {
        public int MedicalFieldId { get; set; }
        public int DoctorId { get; set; }

        public virtual MedicalField MedicalField { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}