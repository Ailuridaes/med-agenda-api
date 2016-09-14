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
    public class MedicalFieldsController : ApiController
    {
        private MedAgendaDataContext db = new MedAgendaDataContext();

        // GET: api/MedicalFields
        public IQueryable<MedicalField> GetMedicalFields()
        {
            return db.MedicalFields;
        }

        // GET: api/MedicalFields/5
        [ResponseType(typeof(MedicalField))]
        public IHttpActionResult GetMedicalField(int id)
        {
            MedicalField medicalField = db.MedicalFields.Find(id);
            if (medicalField == null)
            {
                return NotFound();
            }

            return Ok(medicalField);
        }

        // PUT: api/MedicalFields/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMedicalField(int id, MedicalField medicalField)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != medicalField.MedicalFieldId)
            {
                return BadRequest();
            }

            db.Entry(medicalField).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalFieldExists(id))
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

        // POST: api/MedicalFields
        [ResponseType(typeof(MedicalField))]
        public IHttpActionResult PostMedicalField(MedicalField medicalField)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MedicalFields.Add(medicalField);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = medicalField.MedicalFieldId }, medicalField);
        }

        // DELETE: api/MedicalFields/5
        [ResponseType(typeof(MedicalField))]
        public IHttpActionResult DeleteMedicalField(int id)
        {
            MedicalField medicalField = db.MedicalFields.Find(id);
            if (medicalField == null)
            {
                return NotFound();
            }

            db.MedicalFields.Remove(medicalField);
            db.SaveChanges();

            return Ok(medicalField);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MedicalFieldExists(int id)
        {
            return db.MedicalFields.Count(e => e.MedicalFieldId == id) > 0;
        }
    }
}