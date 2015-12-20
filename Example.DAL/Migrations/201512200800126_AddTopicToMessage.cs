namespace Example.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTopicToMessage : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Message", "Topic_Id", "dbo.Topic");
            DropIndex("dbo.Message", new[] { "Topic_Id" });
            RenameColumn(table: "dbo.Message", name: "Topic_Id", newName: "TopicId");
            AlterColumn("dbo.Message", "TopicId", c => c.Int(nullable: false));
            CreateIndex("dbo.Message", "TopicId");
            AddForeignKey("dbo.Message", "TopicId", "dbo.Topic", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Message", "TopicId", "dbo.Topic");
            DropIndex("dbo.Message", new[] { "TopicId" });
            AlterColumn("dbo.Message", "TopicId", c => c.Int());
            RenameColumn(table: "dbo.Message", name: "TopicId", newName: "Topic_Id");
            CreateIndex("dbo.Message", "Topic_Id");
            AddForeignKey("dbo.Message", "Topic_Id", "dbo.Topic", "Id");
        }
    }
}
