namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DelPropActeMedbis : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Traitements", "ActeMedical_Id", "dbo.ActeMedicals");
            DropIndex("dbo.Traitements", new[] { "ActeMedical_Id" });
            DropColumn("dbo.Traitements", "ActeMedical_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Traitements", "ActeMedical_Id", c => c.Int());
            CreateIndex("dbo.Traitements", "ActeMedical_Id");
            AddForeignKey("dbo.Traitements", "ActeMedical_Id", "dbo.ActeMedicals", "Id");
        }
    }
}
