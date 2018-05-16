namespace CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        BlogId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100, storeType: "nvarchar"),
                        AddUrl = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.BlogId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 100, storeType: "nvarchar"),
                        Content = c.String(maxLength: 100, storeType: "nvarchar"),
                        BlogId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Posts");
            DropTable("dbo.Blogs");
        }
    }
}
