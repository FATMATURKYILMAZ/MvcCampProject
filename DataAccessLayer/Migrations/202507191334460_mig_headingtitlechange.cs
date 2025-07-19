namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig_headingtitlechange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Headings", "HeadingStatus", c => c.Boolean(nullable: false));
            DropColumn("dbo.Headings", "ContentStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Headings", "ContentStatus", c => c.Boolean(nullable: false));
            DropColumn("dbo.Headings", "HeadingStatus");
        }
    }
}
