namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPayementEntite : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PayementOrds", "TraitementId", "dbo.Traitements");
            DropForeignKey("dbo.PayementAssurs", "Traitement_Id", "dbo.Traitements");
            DropIndex("dbo.PayementOrds", new[] { "TraitementId" });
            DropIndex("dbo.PayementAssurs", new[] { "Traitement_Id" });
            CreateTable(
                "dbo.Payements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypePaye = c.String(nullable: false),
                        InfoCheque = c.String(),
                        MontantPaye = c.Single(nullable: false),
                        DatePaye = c.DateTime(nullable: false),
                        DateEnreg = c.DateTime(nullable: false),
                        Del = c.Boolean(nullable: false),
                        SourcePaye = c.Int(nullable: false),
                        TraitementId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Traitements", t => t.TraitementId, cascadeDelete: true)
                .Index(t => t.TraitementId);
            
            DropTable("dbo.PayementOrds");
            DropTable("dbo.PayementAssurs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PayementAssurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypePaye = c.String(),
                        InfoCheque = c.String(),
                        MontantPaye = c.Single(nullable: false),
                        DatePaye = c.DateTime(nullable: false),
                        DateEnreg = c.DateTime(nullable: false),
                        Del = c.Boolean(nullable: false),
                        TaitementId = c.Int(nullable: false),
                        Traitement_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PayementOrds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypePaye = c.String(),
                        InfoCheque = c.String(),
                        MontantPaye = c.Single(nullable: false),
                        DatePaye = c.DateTime(nullable: false),
                        DateEnreg = c.DateTime(nullable: false),
                        Del = c.Boolean(nullable: false),
                        TraitementId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Payements", "TraitementId", "dbo.Traitements");
            DropIndex("dbo.Payements", new[] { "TraitementId" });
            DropTable("dbo.Payements");
            CreateIndex("dbo.PayementAssurs", "Traitement_Id");
            CreateIndex("dbo.PayementOrds", "TraitementId");
            AddForeignKey("dbo.PayementAssurs", "Traitement_Id", "dbo.Traitements", "Id");
            AddForeignKey("dbo.PayementOrds", "TraitementId", "dbo.Traitements", "Id", cascadeDelete: true);
        }
    }
}
