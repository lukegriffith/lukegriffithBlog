namespace lukegriffithBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "timePosted", c => c.DateTime(nullable: false));
            DropColumn("dbo.Posts", "DatePosted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "DatePosted", c => c.DateTime(nullable: false));
            DropColumn("dbo.Posts", "timePosted");
        }
    }
}
