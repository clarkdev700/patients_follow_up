namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutContraintes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Traitements", "AssuranceId", "dbo.Assurances");
            DropIndex("dbo.Traitements", new[] { "AssuranceId" });
            RenameColumn(table: "dbo.DossierPats", name: "Statut", newName: "StatutClosed");
            AddColumn("dbo.DossierPats", "DateEnreg", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ActeMedicals", "Code", c => c.String(nullable: false));
            AlterColumn("dbo.Traitements", "AssuranceId", c => c.Int());
            AlterColumn("dbo.Assurances", "Code", c => c.String(nullable: false));
            AlterColumn("dbo.Assurances", "Nom", c => c.String(nullable: false));
            AlterColumn("dbo.Medecins", "NumMat", c => c.String(nullable: false));
            AlterColumn("dbo.Medecins", "Nom", c => c.String(nullable: false));
            AlterColumn("dbo.Medecins", "Prenoms", c => c.String(nullable: false));
            AlterColumn("dbo.Medecins", "Sexe", c => c.String(maxLength: 1));
            AlterColumn("dbo.Medecins", "DateEntree", c => c.DateTime());
            AlterColumn("dbo.Medecins", "DateSortie", c => c.DateTime());
            AlterColumn("dbo.Patients", "NumMat", c => c.String(nullable: false));
            AlterColumn("dbo.Patients", "Nom", c => c.String(nullable: false));
            AlterColumn("dbo.Patients", "Prenom", c => c.String(nullable: false));
            AlterColumn("dbo.Patients", "Sexe", c => c.String(maxLength: 1));
            CreateIndex("dbo.Traitements", "AssuranceId");
            AddForeignKey("dbo.Traitements", "AssuranceId", "dbo.Assurances", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Traitements", "AssuranceId", "dbo.Assurances");
            DropIndex("dbo.Traitements", new[] { "AssuranceId" });
            AlterColumn("dbo.Patients", "Sexe", c => c.String());
            AlterColumn("dbo.Patients", "Prenom", c => c.String());
            AlterColumn("dbo.Patients", "Nom", c => c.String());
            AlterColumn("dbo.Patients", "NumMat", c => c.Int(nullable: false));
            AlterColumn("dbo.Medecins", "DateSortie", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Medecins", "DateEntree", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Medecins", "Sexe", c => c.String());
            AlterColumn("dbo.Medecins", "Prenoms", c => c.String());
            AlterColumn("dbo.Medecins", "Nom", c => c.String());
            AlterColumn("dbo.Medecins", "NumMat", c => c.Int(nullable: false));
            AlterColumn("dbo.Assurances", "Nom", c => c.String());
            AlterColumn("dbo.Assurances", "Code", c => c.String());
            AlterColumn("dbo.Traitements", "AssuranceId", c => c.Int(nullable: false));
            AlterColumn("dbo.ActeMedicals", "Code", c => c.String());
            DropColumn("dbo.DossierPats", "DateEnreg");
            RenameColumn(table: "dbo.DossierPats", name: "StatutClosed", newName: "Statut");
            CreateIndex("dbo.Traitements", "AssuranceId");
            AddForeignKey("dbo.Traitements", "AssuranceId", "dbo.Assurances", "Id", cascadeDelete: true);
        }
    }
}
