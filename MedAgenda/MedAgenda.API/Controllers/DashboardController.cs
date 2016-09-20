using MedAgenda.API.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MedAgenda.API.Controllers
{
    public class DashboardController : ApiController
    {
        private MedAgendaDataContext _dataContext = new MedAgendaDataContext();

        [HttpGet, Route("api/dashboard/monthlyPatientSummary")]
        public IHttpActionResult GetMonthlyPatientSummary()
        {

            // dynamically get the last 7 days as a string[]
            // Set reference to 7 days ago from today's date.
            var sevenDaysAgo = DateTime.Now.Date.AddDays(-6);

            // Instantiate a new array string called 'days' to hold 7 items
            string[] days = new string[7];
            // Iterate through each 'days' item as long as it is less than 7
            for (int i = 0; i < 7; i++)
            {
                // In the last 7 days reference convert each day of the week to a string, add it to the 'days' array.
                days[i] = sevenDaysAgo.AddDays(i).DayOfWeek.ToString();
            }

            // patient count by day
            int[] patientCountByDay = new int[7];
            // Iterate through all 7 days of the week for each patient
            for (int i = 0; i < 7; i++)
            {
                // Add each day iteration from 7 days ago and add it to startOfDay variable
                var startOfDay = sevenDaysAgo.AddDays(i);
                // Add 1 day to the startOfDay variable but minus 1 second to get 11:59pm to set the endOfDay variable.
                var endOfDay = startOfDay.AddDays(1).AddSeconds(-1);

                patientCountByDay[i] =
                    _dataContext.PatientCheckIns
                                // For all patient checkins count those from 12am to 11:50pm for a particular day.
                                .Count(pc => pc.CheckInTime >= startOfDay && pc.CheckInTime <= endOfDay);
            }

            // doctor count by day
            int[] doctorCountByDay = new int[7];
            for (int i = 0; i < 7; i++)
            {
                var startOfDay = sevenDaysAgo.AddDays(i);
                var endOfDay = startOfDay.AddDays(1).AddSeconds(-1);

                doctorCountByDay[i] =
                    _dataContext.DoctorCheckIns
                                // For all doctors checkins count those from 12am to 11:50pm for a particular day.
                                .Count(dc => dc.CheckInTime >= startOfDay && dc.CheckInTime <= endOfDay);
            }


            // Return data to dashboard metrics
            return Ok(new
            {
                Labels = days,
                // Instaniate a new int array of (2) arrays
                Data = new int[2][]
                {
                    // Total daily patient checkin count
                    patientCountByDay, 
                    // Total daily doctor checkin count
                    doctorCountByDay
                }
            });
        }

        protected override void Dispose(bool disposing)
        {
            _dataContext.Dispose();
        }



        // Get medical field names with associated doctor counts     
        [HttpGet, Route("api/dashboard/doctorSpecialities")]
        public IHttpActionResult GetDoctorSpecialties()
        {

            // Get Medicalfield names assigned to 'labels'
            string[] labels = _dataContext.MedicalFields.Select(mf => mf.Name).ToArray();

            // Instantiate a new int array object assignedS 'data'
            int[] data = new int[labels.Length];

            // Get array values for medical field labels
            for (int i = 0; i < labels.Length; i++)
            {
                // Store each iteration of the labels into a new string label object
                string label = labels[i];
                // Take the medical field names that equal the label iteration, count it and assign each iteration to data array.
                data[i] = _dataContext.Specialties.Count(s => s.MedicalField.Name == label);
            }

            // Return doctor specialty data
            return Ok(new
            {
                Labels = labels,
                Data = data
            });


        }

        // Get Patient conditions with associated patient counts     
        [HttpGet, Route("api/dashboard/patientConditions")]
        public IHttpActionResult GetPatientConditions()
        {
            // Get Medicalfield names assigned to 'labels'
            string[] labels = _dataContext.MedicalFields.Select(mf => mf.Name).ToArray();

            // Instantiate a new int array object assignedS 'data'
            int[] data = new int[labels.Length];
        }

            // Return doctor specialty data
            return Ok(new
            {
                Labels = labels,
                Data = data
            });
}
