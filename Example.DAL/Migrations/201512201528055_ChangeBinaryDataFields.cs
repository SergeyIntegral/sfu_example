namespace Example.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeBinaryDataFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BinaryData", "MimeType", c => c.String());
            DropColumn("dbo.BinaryData", "Generation");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BinaryData", "Generation", c => c.Int());
            DropColumn("dbo.BinaryData", "MimeType");
        }
    }
}
