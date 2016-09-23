namespace MedAgenda.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPatientCheckInResolution : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PatientCheckIns", "Resolution", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatientCheckIns", "Resolution");
        }
    }
}
