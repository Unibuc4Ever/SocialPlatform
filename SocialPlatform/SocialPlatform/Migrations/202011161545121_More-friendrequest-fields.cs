namespace SocialPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Morefriendrequestfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "lastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "ApplicationUser_Id1", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "ApplicationUser_Id2", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "ApplicationUser_Id1");
            CreateIndex("dbo.AspNetUsers", "ApplicationUser_Id2");
            AddForeignKey("dbo.AspNetUsers", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "ApplicationUser_Id2", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ApplicationUser_Id2", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "ApplicationUser_Id2" });
            DropIndex("dbo.AspNetUsers", new[] { "ApplicationUser_Id1" });
            DropColumn("dbo.AspNetUsers", "ApplicationUser_Id2");
            DropColumn("dbo.AspNetUsers", "ApplicationUser_Id1");
            DropColumn("dbo.AspNetUsers", "lastName");
        }
    }
}
