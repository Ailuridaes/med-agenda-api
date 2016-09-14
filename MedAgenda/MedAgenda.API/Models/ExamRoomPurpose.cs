using System.Collections.Generic;

namespace MedAgenda.API.Models
{
    public class ExamRoomPurpose
    {
        public int ExamRoomId { get; set; }
        public int MedicalFieldId { get; set; }

        public virtual ExamRoom ExamRoom { get; set; }
        public virtual MedicalField MedicalField { get; set; }
    }
}