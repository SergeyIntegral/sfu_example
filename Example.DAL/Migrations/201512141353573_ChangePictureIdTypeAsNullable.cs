namespace Example.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePictureIdTypeAsNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Topic", "PictureId", "dbo.BinaryData");
            DropIndex("dbo.Topic", new[] { "PictureId" });
            AlterColumn("dbo.Topic", "PictureId", c => c.Int());
            CreateIndex("dbo.Topic", "PictureId");
            AddForeignKey("dbo.Topic", "PictureId", "dbo.BinaryData", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Topic", "PictureId", "dbo.BinaryData");
            DropIndex("dbo.Topic", new[] { "PictureId" });
            AlterColumn("dbo.Topic", "PictureId", c => c.Int(nullable: false));
            CreateIndex("dbo.Topic", "PictureId");
            AddForeignKey("dbo.Topic", "PictureId", "dbo.BinaryData", "Id", cascadeDelete: true);
        }
    }
}
