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
    public class ExamRoomPurposesController : ApiController
    {
        private MedAgendaDataContext db = new MedAgendaDataContext();

        // GET: api/ExamRoomPurposes
        public IQueryable<ExamRoomPurpose> GetExamRoomPurposes()
        {
            return db.ExamRoomPurposes;
        }

        // GET: api/ExamRoomPurposes/5
        [ResponseType(typeof(ExamRoomPurpose))]
        [HttpGet, Route("api/examroompurposes/{examRoomId}/{medicalFieldId}")]
        public IHttpActionResult GetExamRoomPurpose(int examRoomId, int medicalFieldId)
        {
            //ExamRoomPurpose examRoomPurpose = db.ExamRoomPurposes.Find(id);
            var result = db.ExamRoomPurposes.Where(a => a.ExamRoomId == examRoomId && a.MedicalFieldId == medicalFieldId);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/ExamRoomPurposes/5
        [ResponseType(typeof(void))]
        [HttpGet, Route("api/examroompurposes/{examRoomId}/{medicalFieldId}")]
        public IHttpActionResult PutExamRoomPurpose(int examRoomId, int medicalFieldId, ExamRoomPurpose examRoomPurpose)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (examRoomId != examRoomPurpose.MedicalFieldId || medicalFieldId != examRoomPurpose.MedicalFieldId)
            {
                return BadRequest();
            }

            // db.Entry(examRoomPurpose).State = EntityState.Modified;
            var examRoomPurposeToBeUpdated = db.ExamRoomPurposes.FirstOrDefault(a => a.ExamRoomId == examRoomId && a.MedicalFieldId == medicalFieldId);
           
            db.Entry(examRoomPurposeToBeUpdated).CurrentValues.SetValues(examRoomPurpose);
            db.Entry(examRoomPurposeToBeUpdated).State = EntityState.Modified;


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamRoomPurposeExists(examRoomId, medicalFieldId))
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

        // POST: api/ExamRoomPurposes
        [ResponseType(typeof(ExamRoomPurpose))]
        public IHttpActionResult PostExamRoomPurpose(ExamRoomPurpose examRoomPurpose)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExamRoomPurposes.Add(examRoomPurpose);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ExamRoomPurposeExists(examRoomPurpose.ExamRoomId, examRoomPurpose.MedicalFieldId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = examRoomPurpose.MedicalFieldId }, examRoomPurpose);
        }

        // DELETE: api/ExamRoomPurposes/5
        [ResponseType(typeof(ExamRoomPurpose))]
        [HttpGet, Route("api/examroompurposes/{examRoomId}/{medicalFieldId}")]
        public IHttpActionResult DeleteExamRoomPurpose(int examRoomId, int medicalFieldId)
        {
            ExamRoomPurpose examRoomPurpose = db.ExamRoomPurposes.Find(examRoomId, medicalFieldId);
            if (examRoomPurpose == null)
            {
                return NotFound();
            }

            db.ExamRoomPurposes.Remove(examRoomPurpose);
            db.SaveChanges();

            return Ok(examRoomPurpose);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExamRoomPurposeExists(int examRoomId, int medicalFieldId)
        {
            return db.ExamRoomPurposes.Count(e => e.ExamRoomId == examRoomId && e.MedicalFieldId == medicalFieldId) > 0;
        }
    }
}