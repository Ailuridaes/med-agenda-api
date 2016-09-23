using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MedAgenda.API.Infrastructure;
using MedAgenda.API.Models;

namespace MedAgenda.API.Controllers
{
    public class AssignmentsController : ApiController
    {
        private MedAgendaDataContext db = new MedAgendaDataContext();

        // GET: api/Assignments
        public IQueryable<Assignment> GetAssignments()
        {
            return db.Assignments;
        }

        // GET: api/Assignments/5
        [ResponseType(typeof(Assignment))]
        [HttpGet, Route("api/assignments/{doctorId}/{patientCheckInId}")]
        public IHttpActionResult GetAssignment(int doctorId, int patientCheckInId)
        {
            DoctorCheckIn doctorCheckIn = db.Doctors.Where(d => d.DoctorId == doctorId).First()
               .DoctorCheckIns.Where(c => c.CheckOutTime == null).First();

            if (doctorCheckIn == null)
            {
                return BadRequest("This doctor is not checked in.");
            }

            var result = db.Assignments.Where(a => a.PatientCheckInId == patientCheckInId && a.DoctorCheckInId == doctorCheckIn.DoctorCheckInId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/Assignments/5
        [ResponseType(typeof(void))]
        [HttpPut, Route("api/Assignments/{doctorCheckInId}/{patientCheckInId}")]
        public IHttpActionResult PutAssignment(int doctorCheckInId, int patientCheckInId, Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (patientCheckInId != assignment.PatientCheckInId || doctorCheckInId != assignment.DoctorCheckInId)
            {
                return BadRequest();
            }

            //db.Entry(assignment).State = EntityState.Modified;
            var assignmentToBeUpdated = db.Assignments.FirstOrDefault(a => a.PatientCheckInId == patientCheckInId && a.DoctorCheckInId == doctorCheckInId);

            db.Entry(assignmentToBeUpdated).CurrentValues.SetValues(assignment);
            db.Entry(assignmentToBeUpdated).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(doctorCheckInId, patientCheckInId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Assignments/End/5/6
        [ResponseType(typeof(void))]
        [HttpPut, Route("api/Assignments/End/{doctorCheckInId}/{patientCheckInId}")]
        public IHttpActionResult PutAssignmentEnd(int doctorCheckInId, int patientCheckInId, Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (patientCheckInId != assignment.PatientCheckInId || doctorCheckInId != assignment.DoctorCheckInId)
            {
                return BadRequest();
            }

            // Add EndTime to Assignment
            assignment.EndTime = DateTime.Now;

            //db.Entry(assignment).State = EntityState.Modified;
            var assignmentToBeUpdated = db.Assignments.FirstOrDefault(a => a.PatientCheckInId == patientCheckInId && a.DoctorCheckInId == doctorCheckInId);

            db.Entry(assignmentToBeUpdated).CurrentValues.SetValues(assignment);
            db.Entry(assignmentToBeUpdated).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(doctorCheckInId, patientCheckInId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Assignments
        [ResponseType(typeof(Assignment))]
        public IHttpActionResult PostAssignment(Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Add StartTime to new Assignment
            assignment.StartTime = DateTime.Now;

            db.Assignments.Add(assignment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AssignmentExists(assignment.DoctorCheckInId, assignment.PatientCheckInId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = assignment.PatientCheckInId }, assignment);
        }

        // POST: api/Assignments/3/5
        [HttpPost, Route("api/Assignments/{doctorId}/{patientCheckInId}", Name = "CreateAssignment")]
        [ResponseType(typeof(Assignment))]
        public IHttpActionResult PostAssignmentById(int doctorId, int patientCheckInId)
        {
            DoctorCheckIn doctorCheckIn = db.Doctors.Where(d => d.DoctorId == doctorId).First()
                .DoctorCheckIns.Where(c => c.CheckOutTime == null).First();

            if (doctorCheckIn == null)
            {
                return BadRequest("This doctor is not checked in.");
            }

            Assignment assignment = new Assignment(doctorCheckIn.DoctorCheckInId, patientCheckInId);

            // Add StartTime to new Assignment
            assignment.StartTime = DateTime.Now;

            db.Assignments.Add(assignment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AssignmentExists(assignment.DoctorCheckInId, assignment.PatientCheckInId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("CreateAssignment", new { patientCheckInId = assignment.PatientCheckInId, doctorCheckInId = assignment.DoctorCheckInId }, assignment);
        }

        // DELETE: api/Assignments/5
        [ResponseType(typeof(Assignment))]
        [HttpDelete, Route("api/Assignments/{patientCheckInId}/{doctorCheckInId}")]
        public IHttpActionResult DeleteAssignment(int doctorCheckInId, int patientCheckInId)
        {
            Assignment assignment = db.Assignments.Find(doctorCheckInId, patientCheckInId);
            if (assignment == null)
            {
                return NotFound();
            }

            db.Assignments.Remove(assignment);
            db.SaveChanges();

            return Ok(assignment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AssignmentExists(int doctorCheckInId, int patientCheckInId)
        {
            return db.Assignments.Count(e => e.PatientCheckInId == patientCheckInId && e.DoctorCheckInId == doctorCheckInId) > 0;
        }
    }
}