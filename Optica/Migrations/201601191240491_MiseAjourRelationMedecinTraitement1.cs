namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MiseAjourRelationMedecinTraitement1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Traitements", "MedecinRecommandant_Id", "dbo.Medecins");
            DropForeignKey("dbo.Traitements", "MedecinTraitant_Id", "dbo.Medecins");
            DropIndex("dbo.Traitements", new[] { "MedecinTraitant_Id" });
            DropIndex("dbo.Traitements", new[] { "MedecinRecommandant_Id" });
            RenameColumn(table: "dbo.Traitements", name: "MedecinRecommandant_Id", newName: "MedecinRecommandantId");
            RenameColumn(table: "dbo.Traitements", name: "MedecinTraitant_Id", newName: "MedecinTraitantId");
            AlterColumn("dbo.Traitements", "MedecinTraitantId", c => c.Int(nullable: false));
            AlterColumn("dbo.Traitements", "MedecinRecommandantId", c => c.Int(nullable: false));
            CreateIndex("dbo.Traitements", "MedecinTraitantId");
            CreateIndex("dbo.Traitements", "MedecinRecommandantId");
            AddForeignKey("dbo.Traitements", "MedecinRecommandantId", "dbo.Medecins", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Traitements", "MedecinTraitantId", "dbo.Medecins", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Traitements", "MedecinTraitantId", "dbo.Medecins");
            DropForeignKey("dbo.Traitements", "MedecinRecommandantId", "dbo.Medecins");
            DropIndex("dbo.Traitements", new[] { "MedecinRecommandantId" });
            DropIndex("dbo.Traitements", new[] { "MedecinTraitantId" });
            AlterColumn("dbo.Traitements", "MedecinRecommandantId", c => c.Int());
            AlterColumn("dbo.Traitements", "MedecinTraitantId", c => c.Int());
            RenameColumn(table: "dbo.Traitements", name: "MedecinTraitantId", newName: "MedecinTraitant_Id");
            RenameColumn(table: "dbo.Traitements", name: "MedecinRecommandantId", newName: "MedecinRecommandant_Id");
            CreateIndex("dbo.Traitements", "MedecinRecommandant_Id");
            CreateIndex("dbo.Traitements", "MedecinTraitant_Id");
            AddForeignKey("dbo.Traitements", "MedecinTraitant_Id", "dbo.Medecins", "Id");
            AddForeignKey("dbo.Traitements", "MedecinRecommandant_Id", "dbo.Medecins", "Id");
        }
    }
}
