namespace Example.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SectionChilds1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Section", "ParentId");
            RenameColumn(table: "dbo.Section", name: "Parent_Id", newName: "ParentId");
            RenameIndex(table: "dbo.Section", name: "IX_Parent_Id", newName: "IX_ParentId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Section", name: "IX_ParentId", newName: "IX_Parent_Id");
            RenameColumn(table: "dbo.Section", name: "ParentId", newName: "Parent_Id");
            AddColumn("dbo.Section", "ParentId", c => c.Int());
        }
    }
}
