namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DelPropActeMedical : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Traitements", "ActeMedicalId", "dbo.ActeMedicals");
            DropIndex("dbo.Traitements", new[] { "ActeMedicalId" });
            RenameColumn(table: "dbo.Traitements", name: "ActeMedicalId", newName: "ActeMedical_Id");
            AlterColumn("dbo.Traitements", "ActeMedical_Id", c => c.Int());
            CreateIndex("dbo.Traitements", "ActeMedical_Id");
            AddForeignKey("dbo.Traitements", "ActeMedical_Id", "dbo.ActeMedicals", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Traitements", "ActeMedical_Id", "dbo.ActeMedicals");
            DropIndex("dbo.Traitements", new[] { "ActeMedical_Id" });
            AlterColumn("dbo.Traitements", "ActeMedical_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Traitements", name: "ActeMedical_Id", newName: "ActeMedicalId");
            CreateIndex("dbo.Traitements", "ActeMedicalId");
            AddForeignKey("dbo.Traitements", "ActeMedicalId", "dbo.ActeMedicals", "Id", cascadeDelete: true);
        }
    }
}
