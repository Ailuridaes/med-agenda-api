namespace MedAgenda.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCheckinTimes1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assignments", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Assignments", "EndTime", c => c.DateTime());
            AddColumn("dbo.DoctorCheckIns", "CheckInTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.DoctorCheckIns", "CheckOutTime", c => c.DateTime());
            AddColumn("dbo.PatientCheckIns", "CheckOutTime", c => c.DateTime());
            DropColumn("dbo.Assignments", "CheckInTime");
            DropColumn("dbo.Assignments", "CheckOutTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assignments", "CheckOutTime", c => c.DateTime());
            AddColumn("dbo.Assignments", "CheckInTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.PatientCheckIns", "CheckOutTime");
            DropColumn("dbo.DoctorCheckIns", "CheckOutTime");
            DropColumn("dbo.DoctorCheckIns", "CheckInTime");
            DropColumn("dbo.Assignments", "EndTime");
            DropColumn("dbo.Assignments", "StartTime");
        }
    }
}
