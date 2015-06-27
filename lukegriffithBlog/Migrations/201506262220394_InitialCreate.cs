namespace lukegriffithBlog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        dateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        urlSlug = c.String(),
                        subTitle = c.String(),
                        body = c.String(),
                        published = c.Boolean(nullable: false),
                        DatePosted = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.postCategories",
                c => new
                    {
                        PostID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostID, t.CategoryID })
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.PostID)
                .Index(t => t.CategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.postCategories", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.postCategories", "PostID", "dbo.Posts");
            DropIndex("dbo.postCategories", new[] { "CategoryID" });
            DropIndex("dbo.postCategories", new[] { "PostID" });
            DropTable("dbo.postCategories");
            DropTable("dbo.Posts");
            DropTable("dbo.Category");
        }
    }
}
