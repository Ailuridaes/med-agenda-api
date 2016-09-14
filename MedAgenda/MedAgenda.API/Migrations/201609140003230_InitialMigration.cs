namespace MedAgenda.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        PatientCheckInId = c.Int(nullable: false),
                        DoctorCheckInId = c.Int(nullable: false),
                        ExamRoomId = c.Int(),
                        CheckInTime = c.DateTime(nullable: false),
                        CheckOutTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.PatientCheckInId, t.DoctorCheckInId })
                .ForeignKey("dbo.DoctorCheckIns", t => t.DoctorCheckInId, cascadeDelete: true)
                .ForeignKey("dbo.ExamRooms", t => t.ExamRoomId)
                .ForeignKey("dbo.PatientCheckIns", t => t.PatientCheckInId, cascadeDelete: true)
                .Index(t => t.PatientCheckInId)
                .Index(t => t.DoctorCheckInId)
                .Index(t => t.ExamRoomId);
            
            CreateTable(
                "dbo.DoctorCheckIns",
                c => new
                    {
                        DoctorCheckInId = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DoctorCheckInId)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorId = c.Int(nullable: false, identity: true),
                        ExamRoomId = c.Int(),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(),
                        Telephone = c.String(),
                    })
                .PrimaryKey(t => t.DoctorId)
                .ForeignKey("dbo.ExamRooms", t => t.ExamRoomId)
                .Index(t => t.ExamRoomId);
            
            CreateTable(
                "dbo.ExamRooms",
                c => new
                    {
                        ExamRoomId = c.Int(nullable: false, identity: true),
                        RoomNumber = c.String(),
                    })
                .PrimaryKey(t => t.ExamRoomId);
            
            CreateTable(
                "dbo.ExamRoomPurposes",
                c => new
                    {
                        MedicalFieldId = c.Int(nullable: false),
                        ExamRoomId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MedicalFieldId, t.ExamRoomId })
                .ForeignKey("dbo.MedicalFields", t => t.MedicalFieldId, cascadeDelete: true)
                .ForeignKey("dbo.ExamRooms", t => t.ExamRoomId, cascadeDelete: true)
                .Index(t => t.MedicalFieldId)
                .Index(t => t.ExamRoomId);
            
            CreateTable(
                "dbo.MedicalFields",
                c => new
                    {
                        MedicalFieldId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.MedicalFieldId);
            
            CreateTable(
                "dbo.PatientCheckIns",
                c => new
                    {
                        PatientCheckInId = c.Int(nullable: false, identity: true),
                        MedicalFieldId = c.Int(),
                        PatientId = c.Int(nullable: false),
                        PainScale = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PatientCheckInId)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .ForeignKey("dbo.MedicalFields", t => t.MedicalFieldId)
                .Index(t => t.MedicalFieldId)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Email = c.String(),
                        Telephone = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PatientId);
            
            CreateTable(
                "dbo.EmergencyContacts",
                c => new
                    {
                        EmergencyContactID = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Telephone = c.String(),
                    })
                .PrimaryKey(t => t.EmergencyContactID)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.Specialties",
                c => new
                    {
                        MedicalFieldId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MedicalFieldId, t.DoctorId })
                .ForeignKey("dbo.MedicalFields", t => t.MedicalFieldId, cascadeDelete: true)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.MedicalFieldId)
                .Index(t => t.DoctorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Specialties", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.ExamRoomPurposes", "ExamRoomId", "dbo.ExamRooms");
            DropForeignKey("dbo.Specialties", "MedicalFieldId", "dbo.MedicalFields");
            DropForeignKey("dbo.PatientCheckIns", "MedicalFieldId", "dbo.MedicalFields");
            DropForeignKey("dbo.PatientCheckIns", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.EmergencyContacts", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Assignments", "PatientCheckInId", "dbo.PatientCheckIns");
            DropForeignKey("dbo.ExamRoomPurposes", "MedicalFieldId", "dbo.MedicalFields");
            DropForeignKey("dbo.Doctors", "ExamRoomId", "dbo.ExamRooms");
            DropForeignKey("dbo.Assignments", "ExamRoomId", "dbo.ExamRooms");
            DropForeignKey("dbo.DoctorCheckIns", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Assignments", "DoctorCheckInId", "dbo.DoctorCheckIns");
            DropIndex("dbo.Specialties", new[] { "DoctorId" });
            DropIndex("dbo.Specialties", new[] { "MedicalFieldId" });
            DropIndex("dbo.EmergencyContacts", new[] { "PatientId" });
            DropIndex("dbo.PatientCheckIns", new[] { "PatientId" });
            DropIndex("dbo.PatientCheckIns", new[] { "MedicalFieldId" });
            DropIndex("dbo.ExamRoomPurposes", new[] { "ExamRoomId" });
            DropIndex("dbo.ExamRoomPurposes", new[] { "MedicalFieldId" });
            DropIndex("dbo.Doctors", new[] { "ExamRoomId" });
            DropIndex("dbo.DoctorCheckIns", new[] { "DoctorId" });
            DropIndex("dbo.Assignments", new[] { "ExamRoomId" });
            DropIndex("dbo.Assignments", new[] { "DoctorCheckInId" });
            DropIndex("dbo.Assignments", new[] { "PatientCheckInId" });
            DropTable("dbo.Specialties");
            DropTable("dbo.EmergencyContacts");
            DropTable("dbo.Patients");
            DropTable("dbo.PatientCheckIns");
            DropTable("dbo.MedicalFields");
            DropTable("dbo.ExamRoomPurposes");
            DropTable("dbo.ExamRooms");
            DropTable("dbo.Doctors");
            DropTable("dbo.DoctorCheckIns");
            DropTable("dbo.Assignments");
        }
    }
}
