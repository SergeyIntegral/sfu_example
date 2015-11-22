namespace Example.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BinaryData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.Binary(),
                        Generation = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                        Topic_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.Author_Id)
                .ForeignKey("dbo.Topic", t => t.Topic_Id)
                .Index(t => t.Author_Id)
                .Index(t => t.Topic_Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Index = c.Guid(nullable: false, identity: true),
                        UserName = c.String(),
                        FullName = c.String(maxLength: 450),
                        Role = c.Int(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        LockoutEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTimeOffset(precision: 7),
                        PasswordHash = c.String(),
                        Phone = c.String(),
                        DateOfLastVisit = c.DateTimeOffset(precision: 7),
                        IsEndSession = c.Boolean(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Avatar_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BinaryData", t => t.Avatar_Id)
                .Index(t => t.Avatar_Id);
            
            CreateTable(
                "dbo.Section",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        ParentId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Section", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Topic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Text = c.String(),
                        Status = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                        SectionId = c.Int(nullable: false),
                        PictureId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.Author_Id)
                .ForeignKey("dbo.BinaryData", t => t.PictureId, cascadeDelete: true)
                .ForeignKey("dbo.Section", t => t.SectionId, cascadeDelete: true)
                .Index(t => t.SectionId)
                .Index(t => t.PictureId)
                .Index(t => t.Author_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Topic", "SectionId", "dbo.Section");
            DropForeignKey("dbo.Topic", "PictureId", "dbo.BinaryData");
            DropForeignKey("dbo.Message", "Topic_Id", "dbo.Topic");
            DropForeignKey("dbo.Topic", "Author_Id", "dbo.User");
            DropForeignKey("dbo.Section", "ParentId", "dbo.Section");
            DropForeignKey("dbo.Message", "Author_Id", "dbo.User");
            DropForeignKey("dbo.User", "Avatar_Id", "dbo.BinaryData");
            DropIndex("dbo.Topic", new[] { "Author_Id" });
            DropIndex("dbo.Topic", new[] { "PictureId" });
            DropIndex("dbo.Topic", new[] { "SectionId" });
            DropIndex("dbo.Section", new[] { "ParentId" });
            DropIndex("dbo.User", new[] { "Avatar_Id" });
            DropIndex("dbo.Message", new[] { "Topic_Id" });
            DropIndex("dbo.Message", new[] { "Author_Id" });
            DropTable("dbo.Topic");
            DropTable("dbo.Section");
            DropTable("dbo.User");
            DropTable("dbo.Message");
            DropTable("dbo.BinaryData");
        }
    }
}
