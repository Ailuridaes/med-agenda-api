namespace MedAgenda.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveEmergencyContactFieldsFromPatientTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Patients", "EmergencyContactName");
            DropColumn("dbo.Patients", "EmergencyContactPhone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Patients", "EmergencyContactPhone", c => c.String());
            AddColumn("dbo.Patients", "EmergencyContactName", c => c.String());
        }
    }
}
