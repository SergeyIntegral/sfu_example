namespace Example.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SectionChilds : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Section", "ParentId", "dbo.Section");
            DropIndex("dbo.Section", new[] { "ParentId" });
            AddColumn("dbo.Section", "Parent_Id", c => c.Int());
            CreateIndex("dbo.Section", "Parent_Id");
            AddForeignKey("dbo.Section", "Parent_Id", "dbo.Section", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Section", "Parent_Id", "dbo.Section");
            DropIndex("dbo.Section", new[] { "Parent_Id" });
            DropColumn("dbo.Section", "Parent_Id");
            CreateIndex("dbo.Section", "ParentId");
            AddForeignKey("dbo.Section", "ParentId", "dbo.Section", "Id");
        }
    }
}
