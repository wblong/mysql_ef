namespace CodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_relationship : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Posts", "BlogId");
            AddForeignKey("dbo.Posts", "BlogId", "dbo.Blogs", "BlogId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "BlogId", "dbo.Blogs");
            DropIndex("dbo.Posts", new[] { "BlogId" });
        }
    }
}
