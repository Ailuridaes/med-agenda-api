namespace MedAgenda.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientCheckInSymptoms : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientCheckIns", "Symptoms", c => c.String());
            DropColumn("dbo.Patients", "Symptoms");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patients", "Symptoms", c => c.String());
            DropColumn("dbo.PatientCheckIns", "Symptoms");
        }
    }
}
