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
    public class PatientsController : ApiController
    {
        private MedAgendaDataContext db = new MedAgendaDataContext();

        // GET: api/Patients
        public IQueryable<Patient> GetPatients()
        {
            return db.Patients.Where(d => !d.IsDisabled);
        }

        // GET: api/Patients/all
        [HttpGet, Route("api/patients/all")]
        public IQueryable<Patient> GetAllPatients()
        {
            return db.Patients;
        }

        // GET: api/Patients/5
        [ResponseType(typeof(Patient))]
        public IHttpActionResult GetPatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        // PUT: api/Patients/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPatient(int id, Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patient.PatientId)
            {
                return BadRequest();
            }

            //db.Entry(patient).State = EntityState.Modified;

            var dbPatient = db.Patients.Find(id);

            db.Entry(dbPatient).CurrentValues.SetValues(patient);
            db.Entry(dbPatient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        // POST: api/Patients
        [ResponseType(typeof(Patient))]
        public IHttpActionResult PostPatient(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Patients.Add(patient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patient.PatientId }, patient);
        }

        // DELETE: api/Patients/5
        [ResponseType(typeof(Patient))]
        public IHttpActionResult DeletePatient(int id)
        {
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }

            db.Patients.Remove(patient);
            db.SaveChanges();

            return Ok(patient);
        }

        // Check Patient Exists: GET api/Patients
        [ResponseType(typeof(bool))]
        [HttpGet, Route("api/patients/isreturning/{firstName}/{lastName}/{email}")]
        public IHttpActionResult IsPatientReturning(string firstName, string lastName, string email) 
        {

            
            var isReturning = db.Patients.Count(p => p.FirstName.ToLower() == firstName.ToLower() && p.LastName.ToLower() == lastName.ToLower() && p.Email.ToLower() == email.ToLower());

            if (isReturning == 1)
            {
                var result = db.Patients.FirstOrDefault(p => p.FirstName.ToLower() == firstName.ToLower() && p.LastName.ToLower() == lastName.ToLower() && p.Email.ToLower() == email.ToLower());
                return Ok(result);
            }
            else
            {
                var result = 0;
                return Ok(result);
            }

            
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientExists(int id)
        {
            return db.Patients.Count(e => e.PatientId == id) > 0;
        }
    }
}