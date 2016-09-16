namespace MedAgenda.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixPatientCheckinDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientCheckIns", "CheckInTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Patients", "Medications", c => c.String());
            AddColumn("dbo.Patients", "ChronicConditions", c => c.String());
            AddColumn("dbo.Patients", "Allergies", c => c.String());
            AddColumn("dbo.Patients", "Symptoms", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patients", "Symptoms");
            DropColumn("dbo.Patients", "Allergies");
            DropColumn("dbo.Patients", "ChronicConditions");
            DropColumn("dbo.Patients", "Medications");
            DropColumn("dbo.PatientCheckIns", "CheckInTime");
        }
    }
}
