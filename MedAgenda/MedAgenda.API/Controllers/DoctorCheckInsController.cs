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
    public class DoctorCheckInsController : ApiController
    {
        private MedAgendaDataContext db = new MedAgendaDataContext();

        // GET: api/DoctorCheckIns
        public IQueryable<DoctorCheckIn> GetDoctorCheckIns()
        {
            return db.DoctorCheckIns;
        }

        // GET: api/DoctorCheckIns/5
        [HttpGet]
        [ResponseType(typeof(DoctorCheckIn))]
        public IHttpActionResult GetDoctorCheckIn(int id)
        {
            DoctorCheckIn doctorCheckIn = db.DoctorCheckIns.Find(id);
            if (doctorCheckIn == null)
            {
                return NotFound();
            }

            return Ok(doctorCheckIn);
        }

        // GET: api/DoctorCheckIns/Active
        [HttpGet]
        [Route("api/DoctorCheckIns/Active")]
        [ResponseType(typeof(IQueryable<DoctorCheckIn>))]
        public IHttpActionResult GetDoctorCheckInsActive()
        {
            IQueryable<DoctorCheckIn> doctorCheckIns = db.DoctorCheckIns.Where(d => d.CheckOutTime == null);

            return Ok(doctorCheckIns);
        }

        // PUT: api/DoctorCheckIns/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDoctorCheckIn(int id, DoctorCheckIn doctorCheckIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctorCheckIn.DoctorCheckInId)
            {
                return BadRequest();
            }

            db.Entry(doctorCheckIn).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorCheckInExists(id))
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

        // PUT: api/DoctorCheckIns/CheckOut
        [HttpPut]
        [Route("api/DoctorCheckIns/CheckOut/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDoctorCheckInCheckOut(int id, DoctorCheckIn doctorCheckIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctorCheckIn.DoctorCheckInId)
            {
                return BadRequest();
            }

            // Add CheckOutTime
            doctorCheckIn.CheckOutTime = DateTime.Now;

            db.Entry(doctorCheckIn).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorCheckInExists(id))
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

        // POST: api/DoctorCheckIns
        [ResponseType(typeof(DoctorCheckIn))]
        public IHttpActionResult PostDoctorCheckIn(DoctorCheckIn doctorCheckIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if active checkIn exists for doctor
            if (ActiveCheckinExists(doctorCheckIn.DoctorId))
            {
                return BadRequest("An active checkin already exists for this doctor.");
            }

            // Add CheckInTime to new CheckIn
            doctorCheckIn.CheckInTime = DateTime.Now;

            db.DoctorCheckIns.Add(doctorCheckIn);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctorCheckIn.DoctorCheckInId }, doctorCheckIn);
        }

        // DELETE: api/DoctorCheckIns/5
        [ResponseType(typeof(DoctorCheckIn))]
        public IHttpActionResult DeleteDoctorCheckIn(int id)
        {
            DoctorCheckIn doctorCheckIn = db.DoctorCheckIns.Find(id);
            if (doctorCheckIn == null)
            {
                return NotFound();
            }

            db.DoctorCheckIns.Remove(doctorCheckIn);
            db.SaveChanges();

            return Ok(doctorCheckIn);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoctorCheckInExists(int id)
        {
            return db.DoctorCheckIns.Count(e => e.DoctorCheckInId == id) > 0;
        }

        private bool ActiveCheckinExists(int doctorId)
        {
            return db.DoctorCheckIns.Count(d => d.DoctorId == doctorId && d.CheckOutTime == null) > 0;
        }
    }
}