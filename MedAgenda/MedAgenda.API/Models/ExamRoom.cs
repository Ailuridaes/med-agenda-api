using System.Collections.Generic;

namespace MedAgenda.API.Models
{
    public class ExamRoom
    {
        public int ExamRoomId { get; set; }
        public string RoomNumber { get; set; }

        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<ExamRoomPurpose> ExamRoomPurposes { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}