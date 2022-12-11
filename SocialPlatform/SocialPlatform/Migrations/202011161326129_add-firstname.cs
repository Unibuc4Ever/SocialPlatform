namespace SocialPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfirstname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "firstName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "firstName");
        }
    }
}
