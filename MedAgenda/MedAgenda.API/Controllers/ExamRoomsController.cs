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
    public class ExamRoomsController : ApiController
    {
        private MedAgendaDataContext db = new MedAgendaDataContext();

        // GET: api/ExamRooms
        public IQueryable<ExamRoom> GetExamRooms()
        {
            return db.ExamRooms;
        }

        // GET: api/ExamRooms/5
        [ResponseType(typeof(ExamRoom))]
        public IHttpActionResult GetExamRoom(int id)
        {
            ExamRoom examRoom = db.ExamRooms.Find(id);
            if (examRoom == null)
            {
                return NotFound();
            }

            return Ok(examRoom);
        }

        // PUT: api/ExamRooms/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutExamRoom(int id, ExamRoom examRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != examRoom.ExamRoomId)
            {
                return BadRequest();
            }

            db.Entry(examRoom).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamRoomExists(id))
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

        // POST: api/ExamRooms
        [ResponseType(typeof(ExamRoom))]
        public IHttpActionResult PostExamRoom(ExamRoom examRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ExamRooms.Add(examRoom);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = examRoom.ExamRoomId }, examRoom);
        }

        // DELETE: api/ExamRooms/5
        [ResponseType(typeof(ExamRoom))]
        public IHttpActionResult DeleteExamRoom(int id)
        {
            ExamRoom examRoom = db.ExamRooms.Find(id);
            if (examRoom == null)
            {
                return NotFound();
            }

            db.ExamRooms.Remove(examRoom);
            db.SaveChanges();

            return Ok(examRoom);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExamRoomExists(int id)
        {
            return db.ExamRooms.Count(e => e.ExamRoomId == id) > 0;
        }
    }
}