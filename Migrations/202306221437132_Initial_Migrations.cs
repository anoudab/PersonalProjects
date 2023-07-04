namespace BMS_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial_Migrations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Trips", "StartTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Trips", "ArrivalTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Trips", "Time");
            DropColumn("dbo.Trips", "Duration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Trips", "Duration", c => c.DateTime(nullable: false));
            AddColumn("dbo.Trips", "Time", c => c.DateTime(nullable: false));
            DropColumn("dbo.Trips", "ArrivalTime");
            DropColumn("dbo.Trips", "StartTime");
        }
    }
}
