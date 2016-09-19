namespace MedAgenda.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SyncPatientFieldsWithFrontEnd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "Address", c => c.String());
            AddColumn("dbo.Patients", "EmergencyContactName", c => c.String());
            AddColumn("dbo.Patients", "EmergencyContactPhone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patients", "EmergencyContactPhone");
            DropColumn("dbo.Patients", "EmergencyContactName");
            DropColumn("dbo.Patients", "Address");
        }
    }
}
