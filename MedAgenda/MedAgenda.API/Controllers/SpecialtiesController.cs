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
    public class SpecialtiesController : ApiController
    {
        private MedAgendaDataContext db = new MedAgendaDataContext();

        // GET: api/Specialties
        public IQueryable<Specialty> GetSpecialties()
        {
            return db.Specialties;
        }

        // GET: api/Specialties/5
        [ResponseType(typeof(Specialty))]
        [HttpGet, Route("api/specialties/{doctorId}/{medicalFieldId}")]
        public IHttpActionResult GetSpecialty(int doctorId, int medicalFieldId)
        {
            //Specialty specialty = db.Specialties.Find(doctorId, medicalFieldId);

            var result = db.Specialties.Where(a => a.DoctorId == doctorId && a.MedicalFieldId == medicalFieldId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/Specialties/5
        [ResponseType(typeof(void))]
        [HttpPut, Route("api/specialties/{doctorId}/{medicalFieldId}")]
        public IHttpActionResult PutSpecialty(int doctorId, int medicalFieldId, Specialty specialty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (doctorId != specialty.DoctorId || medicalFieldId != specialty.MedicalFieldId)
            {
                return BadRequest();
            }

            //db.Entry(specialty).State = EntityState.Modified;
            var specialtyToBeUpdated = db.Specialties.FirstOrDefault(a => a.DoctorId == doctorId && a.MedicalFieldId == medicalFieldId);

            db.Entry(specialtyToBeUpdated).CurrentValues.SetValues(specialty);
            db.Entry(specialtyToBeUpdated).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialtyExists(doctorId, medicalFieldId))
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

        // POST: api/Specialties
        [ResponseType(typeof(Specialty))]
        public IHttpActionResult PostSpecialty(Specialty specialty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Specialties.Add(specialty);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (SpecialtyExists(specialty.DoctorId, specialty.MedicalFieldId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = specialty.MedicalFieldId }, specialty);
        }

        // DELETE: api/Specialties/5
        [ResponseType(typeof(Specialty))]
        [HttpDelete, Route("api/specialties/{doctorId}/{medicalFieldId}")]
        public IHttpActionResult DeleteSpecialty(int doctorId, int medicalFieldId)
        {
            Specialty specialty = db.Specialties.Find(doctorId, medicalFieldId);
            if (specialty == null)
            {
                return NotFound();
            }

            db.Specialties.Remove(specialty);
            db.SaveChanges();

            return Ok(specialty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpecialtyExists(int doctorId, int medicalFieldId)
        {
            return db.Specialties.Count(e => e.DoctorId == doctorId && e.MedicalFieldId == medicalFieldId) > 0;
        }
    }
}