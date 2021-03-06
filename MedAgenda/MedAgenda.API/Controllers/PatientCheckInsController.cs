﻿using System;
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
using Twilio;
using MedAgenda.API.HelperFunctions;

namespace MedAgenda.API.Controllers
{
    public class PatientCheckInsController : ApiController
    {
        private MedAgendaDataContext db = new MedAgendaDataContext();

        // GET: api/PatientCheckIns
        public IQueryable<PatientCheckIn> GetPatientCheckIns()
        {
            return db.PatientCheckIns;
        }

        // GET: api/PatientCheckIns/5
        [ResponseType(typeof(PatientCheckIn))]
        public IHttpActionResult GetPatientCheckIn(int id)
        {
            PatientCheckIn patientCheckIn = db.PatientCheckIns.Find(id);
            if (patientCheckIn == null)
            {
                return NotFound();
            }

            return Ok(patientCheckIn);
        }

        // GET: api/PatientCheckIns/Queue
        [HttpGet]
        [ActionName("Queue")]
        [Route("api/PatientCheckIns/Queue/{doctorId?}")]
        [ResponseType(typeof(IQueryable<PatientCheckIn>))]
        public IHttpActionResult GetPatientCheckInsQueue(int? doctorId = null)
        {

            IQueryable<PatientCheckIn> patientCheckIns = db.PatientCheckIns
                .Where(p => p.CheckOutTime == null);
                
            if (doctorId != null)
            {
                var medFields = db.Doctors.Where(d => d.DoctorId == doctorId).First()
                    .Specialties.Select(s => s.MedicalFieldId);
                patientCheckIns = patientCheckIns.Where(p => medFields.Contains(p.MedicalFieldId.Value));
            }
                
            patientCheckIns = patientCheckIns.OrderByDescending(p => p.PainScale);

            return Ok(patientCheckIns);
        }

        // PUT: api/PatientCheckIns/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPatientCheckIn(int id, PatientCheckIn patientCheckIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patientCheckIn.PatientCheckInId)
            {
                return BadRequest();
            }

            var checkInToBeUpdated = db.PatientCheckIns.FirstOrDefault(a => a.PatientCheckInId == id);

            db.Entry(checkInToBeUpdated).CurrentValues.SetValues(patientCheckIn);
            db.Entry(checkInToBeUpdated).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientCheckInExists(id))
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

        // PUT: api/PatientCheckIns/CheckOut
        [HttpPut]
        [Route("api/PatientCheckIns/CheckOut/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPatientCheckInCheckOut(int id, PatientCheckIn patientCheckIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patientCheckIn.PatientCheckInId)
            {
                return BadRequest();
            }

            // Add CheckOutTime
            patientCheckIn.CheckOutTime = DateTime.Now;
            
            var checkInToBeUpdated = db.PatientCheckIns.FirstOrDefault(a => a.PatientCheckInId == id);

            db.Entry(checkInToBeUpdated).CurrentValues.SetValues(patientCheckIn);
            db.Entry(checkInToBeUpdated).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientCheckInExists(id))
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

       

        // POST: api/PatientCheckIns
        [ResponseType(typeof(PatientCheckIn))]
        public IHttpActionResult PostPatientCheckIn(PatientCheckIn patientCheckIn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if active checkIn exists for patient
            if (ActiveCheckinExists(patientCheckIn.PatientId))
            {
                return BadRequest("An active checkin already exists for this patient.");
            }

            // Add CheckInTime to new CheckIn
            patientCheckIn.CheckInTime = DateTime.Now;

            db.PatientCheckIns.Add(patientCheckIn);
            db.SaveChanges();

            var patient = db.Patients.FirstOrDefault(p => p.PatientId == patientCheckIn.PatientId);
            var messageToSend = "Hello " + patient.FirstName + " " + patient.LastName + ". " + "A doctor will be with you shortly. Please have a seat.";

            TwilioSmsHelper.SendSms(patient.Telephone, messageToSend);

            return CreatedAtRoute("DefaultApi", new { id = patientCheckIn.PatientCheckInId }, patientCheckIn);
        }

        // DELETE: api/PatientCheckIns/5
        [ResponseType(typeof(PatientCheckIn))]
        public IHttpActionResult DeletePatientCheckIn(int id)
        {
            PatientCheckIn patientCheckIn = db.PatientCheckIns.Find(id);
            if (patientCheckIn == null)
            {
                return NotFound();
            }

            db.PatientCheckIns.Remove(patientCheckIn);
            db.SaveChanges();

            return Ok(patientCheckIn);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientCheckInExists(int id)
        {
            return db.PatientCheckIns.Count(e => e.PatientCheckInId == id) > 0;
        }

        private bool ActiveCheckinExists(int patientId)
        {
            return db.PatientCheckIns.Count(d => d.PatientId == patientId && d.CheckOutTime == null) > 0;
        }
    }
}