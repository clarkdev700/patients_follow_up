namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TraitementActemicalRelation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TraitementActeMedicals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TraitementId = c.Int(nullable: false),
                        ActeMedicalId = c.Int(nullable: false),
                        Del = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActeMedicals", t => t.ActeMedicalId, cascadeDelete: true)
                .ForeignKey("dbo.Traitements", t => t.TraitementId, cascadeDelete: true)
                .Index(t => t.TraitementId)
                .Index(t => t.ActeMedicalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TraitementActeMedicals", "TraitementId", "dbo.Traitements");
            DropForeignKey("dbo.TraitementActeMedicals", "ActeMedicalId", "dbo.ActeMedicals");
            DropIndex("dbo.TraitementActeMedicals", new[] { "ActeMedicalId" });
            DropIndex("dbo.TraitementActeMedicals", new[] { "TraitementId" });
            DropTable("dbo.TraitementActeMedicals");
        }
    }
}
