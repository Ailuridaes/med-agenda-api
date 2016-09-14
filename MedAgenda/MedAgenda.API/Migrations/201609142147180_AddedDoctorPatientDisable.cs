namespace MedAgenda.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDoctorPatientDisable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "IsDisabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Patients", "IsDisabled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patients", "IsDisabled");
            DropColumn("dbo.Doctors", "IsDisabled");
        }
    }
}
