namespace MedAgenda.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableCheckoutTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assignments", "CheckOutTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Assignments", "CheckOutTime", c => c.DateTime(nullable: false));
        }
    }
}
