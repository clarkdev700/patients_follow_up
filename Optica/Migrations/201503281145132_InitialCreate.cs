namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActeMedicals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Designation = c.String(),
                        DateEnreg = c.DateTime(nullable: false),
                        Del = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Traitements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PrixAssur = c.Single(nullable: false),
                        PrixOrd = c.Single(nullable: false),
                        MontantPaye = c.Single(nullable: false),
                        PayeOrd = c.Boolean(nullable: false),
                        PayeAssur = c.Boolean(nullable: false),
                        Resultat = c.String(),
                        Remarque = c.String(),
                        Recommandation = c.String(),
                        DateTrait = c.DateTime(nullable: false),
                        DateEnregTrait = c.DateTime(nullable: false),
                        MedecinId = c.Int(nullable: false),
                        ActeMedicalId = c.Int(nullable: false),
                        AssuranceId = c.Int(nullable: false),
                        DossierPatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActeMedicals", t => t.ActeMedicalId, cascadeDelete: true)
                .ForeignKey("dbo.Assurances", t => t.AssuranceId, cascadeDelete: true)
                .ForeignKey("dbo.Medecins", t => t.MedecinId, cascadeDelete: true)
                .ForeignKey("dbo.DossierPats", t => t.DossierPatId, cascadeDelete: true)
                .Index(t => t.ActeMedicalId)
                .Index(t => t.AssuranceId)
                .Index(t => t.MedecinId)
                .Index(t => t.DossierPatId);
            
            CreateTable(
                "dbo.Assurances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Nom = c.String(),
                        DateEnreg = c.DateTime(nullable: false),
                        Del = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DossierPats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Motif = c.String(),
                        Statut = c.Boolean(nullable: false),
                        PatientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.ControlePats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateControle = c.DateTime(nullable: false),
                        DateEffectuerControle = c.DateTime(nullable: false),
                        DateEnreg = c.DateTime(nullable: false),
                        MedecinId = c.Int(nullable: false),
                        DossierPatId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DossierPats", t => t.DossierPatId, cascadeDelete: true)
                .ForeignKey("dbo.Medecins", t => t.MedecinId, cascadeDelete: true)
                .Index(t => t.DossierPatId)
                .Index(t => t.MedecinId);
            
            CreateTable(
                "dbo.Medecins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumMat = c.Int(nullable: false),
                        Nom = c.String(),
                        Prenoms = c.String(),
                        Sexe = c.String(),
                        Titre = c.String(),
                        DateEntree = c.DateTime(nullable: false),
                        DateSortie = c.DateTime(nullable: false),
                        DateEnreg = c.DateTime(nullable: false),
                        Del = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumMat = c.Int(nullable: false),
                        Nom = c.String(),
                        Prenom = c.String(),
                        Sexe = c.String(),
                        DateEnreg = c.DateTime(nullable: false),
                        Del = c.Boolean(nullable: false),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Traitements", t => t.TraitementId, cascadeDelete: true)
                .Index(t => t.TraitementId);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Traitements", t => t.Traitement_Id)
                .Index(t => t.Traitement_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PayementAssurs", "Traitement_Id", "dbo.Traitements");
            DropForeignKey("dbo.PayementOrds", "TraitementId", "dbo.Traitements");
            DropForeignKey("dbo.Traitements", "DossierPatId", "dbo.DossierPats");
            DropForeignKey("dbo.DossierPats", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Traitements", "MedecinId", "dbo.Medecins");
            DropForeignKey("dbo.ControlePats", "MedecinId", "dbo.Medecins");
            DropForeignKey("dbo.ControlePats", "DossierPatId", "dbo.DossierPats");
            DropForeignKey("dbo.Traitements", "AssuranceId", "dbo.Assurances");
            DropForeignKey("dbo.Traitements", "ActeMedicalId", "dbo.ActeMedicals");
            DropIndex("dbo.PayementAssurs", new[] { "Traitement_Id" });
            DropIndex("dbo.PayementOrds", new[] { "TraitementId" });
            DropIndex("dbo.Traitements", new[] { "DossierPatId" });
            DropIndex("dbo.DossierPats", new[] { "PatientId" });
            DropIndex("dbo.Traitements", new[] { "MedecinId" });
            DropIndex("dbo.ControlePats", new[] { "MedecinId" });
            DropIndex("dbo.ControlePats", new[] { "DossierPatId" });
            DropIndex("dbo.Traitements", new[] { "AssuranceId" });
            DropIndex("dbo.Traitements", new[] { "ActeMedicalId" });
            DropTable("dbo.PayementAssurs");
            DropTable("dbo.PayementOrds");
            DropTable("dbo.Patients");
            DropTable("dbo.Medecins");
            DropTable("dbo.ControlePats");
            DropTable("dbo.DossierPats");
            DropTable("dbo.Assurances");
            DropTable("dbo.Traitements");
            DropTable("dbo.ActeMedicals");
        }
    }
}
