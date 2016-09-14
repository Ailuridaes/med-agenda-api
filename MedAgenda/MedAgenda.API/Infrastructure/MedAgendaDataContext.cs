using MedAgenda.API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MedAgenda.API.Infrastructure
{
    public class MedAgendaDataContext : DbContext
    {
        public MedAgendaDataContext() :base("MedAgendaDatabase")
        {
           

        }

        public IDbSet<Patient> Patients { get; set; }
        public IDbSet<PatientCheckIn> PatientCheckIns { get; set; }
        public IDbSet<Assignment> Assignments { get; set; }
        public IDbSet<Doctor> Doctors { get; set; }
        public IDbSet<DoctorCheckIn> DoctorCheckIns { get; set; }
        public IDbSet<EmergencyContact> EmergencyContacts { get; set; }
        public IDbSet<ExamRoom> ExamRooms { get; set; }
        public IDbSet<ExamRoomPurpose> ExamRoomPurposes { get; set; }
        public IDbSet<MedicalField> MedicalFields { get; set; }
        public IDbSet<Specialty> Specialties { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Assignment>()
                       
                .HasKey(a => new { a.PatientCheckInId, a.DoctorCheckInId });

            modelBuilder.Entity<Specialty>()

                .HasKey(s => new { s.MedicalFieldId, s.DoctorId });

            modelBuilder.Entity<ExamRoomPurpose>()

                .HasKey(e => new { e.MedicalFieldId, e.ExamRoomId });

            modelBuilder.Entity<PatientCheckIn>()

                .HasMany(p => p.Assignments)
                .WithRequired(a => a.PatientCheckIn)
                .HasForeignKey(a => a.PatientCheckInId);

            modelBuilder.Entity<DoctorCheckIn>()

                .HasMany(p => p.Assignments)
                .WithRequired(a => a.DoctorCheckIn)
                .HasForeignKey(a => a.DoctorCheckInId);

            modelBuilder.Entity<ExamRoom>()

                .HasMany(e => e.Assignments)
                .WithOptional(a => a.ExamRoom)
                .HasForeignKey(a => a.ExamRoomId);

            modelBuilder.Entity<ExamRoom>()

                .HasMany(e => e.Doctors)
                .WithOptional(d => d.ExamRoom)//Changed from required to optional
                .HasForeignKey(d => d.ExamRoomId);

            modelBuilder.Entity<ExamRoom>()

                .HasMany(e => e.ExamRoomPurposes)
                .WithRequired(d => d.ExamRoom)
                .HasForeignKey(d => d.ExamRoomId);

            modelBuilder.Entity<Doctor>()

                .HasMany(e => e.DoctorCheckIns)
                .WithRequired(d => d.Doctor)
                .HasForeignKey(d => d.DoctorId);

            modelBuilder.Entity<Doctor>()

                .HasMany(e => e.Specialties)
                .WithRequired(d => d.Doctor)
                .HasForeignKey(d => d.DoctorId);

            modelBuilder.Entity<Patient>()

                .HasMany(e => e.PatientCheckIns)
                .WithRequired(d => d.Patient)
                .HasForeignKey(d => d.PatientId);

            modelBuilder.Entity<Patient>()

                .HasMany(e => e.EmergencyContacts)
                .WithRequired(d => d.Patient)
                .HasForeignKey(d => d.PatientId);

            modelBuilder.Entity<MedicalField>()

                .HasMany(e => e.Specialties)
                .WithRequired(d => d.MedicalField)
                .HasForeignKey(d => d.MedicalFieldId);

            modelBuilder.Entity<MedicalField>()

                .HasMany(e => e.PatientCheckIns)
                .WithOptional(d => d.MedicalField)
                .HasForeignKey(d => d.MedicalFieldId);

            modelBuilder.Entity<MedicalField>()

                .HasMany(e => e.ExamRoomPurposes)
                .WithRequired(d => d.MedicalField)
                .HasForeignKey(d => d.MedicalFieldId);



        }
    }

    
}