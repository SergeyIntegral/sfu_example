namespace Example.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAuthorIdType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Message", "Author_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Topic", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Message", new[] { "Author_Id" });
            DropIndex("dbo.Topic", new[] { "Author_Id" });
            DropColumn("dbo.Message", "AuthorId");
            DropColumn("dbo.Topic", "AuthorId");
            RenameColumn(table: "dbo.Message", name: "Author_Id", newName: "AuthorId");
            RenameColumn(table: "dbo.Topic", name: "Author_Id", newName: "AuthorId");
            AlterColumn("dbo.Message", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Message", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Topic", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Topic", "AuthorId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Message", "AuthorId");
            CreateIndex("dbo.Topic", "AuthorId");
            AddForeignKey("dbo.Message", "AuthorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Topic", "AuthorId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Topic", "AuthorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Message", "AuthorId", "dbo.AspNetUsers");
            DropIndex("dbo.Topic", new[] { "AuthorId" });
            DropIndex("dbo.Message", new[] { "AuthorId" });
            AlterColumn("dbo.Topic", "AuthorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Topic", "AuthorId", c => c.Int(nullable: false));
            AlterColumn("dbo.Message", "AuthorId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Message", "AuthorId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Topic", name: "AuthorId", newName: "Author_Id");
            RenameColumn(table: "dbo.Message", name: "AuthorId", newName: "Author_Id");
            AddColumn("dbo.Topic", "AuthorId", c => c.Int(nullable: false));
            AddColumn("dbo.Message", "AuthorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Topic", "Author_Id");
            CreateIndex("dbo.Message", "Author_Id");
            AddForeignKey("dbo.Topic", "Author_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Message", "Author_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
