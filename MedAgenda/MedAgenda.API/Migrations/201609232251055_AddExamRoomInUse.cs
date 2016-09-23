namespace MedAgenda.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExamRoomInUse : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExamRooms", "IsInUse", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ExamRooms", "IsInUse");
        }
    }
}
