namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MiseAjourRelationMedecinTraitement : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Traitements", "MedecinId", "dbo.Medecins");
            DropIndex("dbo.Traitements", new[] { "MedecinId" });
            RenameColumn(table: "dbo.Traitements", name: "MedecinId", newName: "MedecinTraitant_Id");
            AddColumn("dbo.Traitements", "MedecinRecommandant_Id", c => c.Int());
            AlterColumn("dbo.Traitements", "MedecinTraitant_Id", c => c.Int());
            CreateIndex("dbo.Traitements", "MedecinTraitant_Id");
            CreateIndex("dbo.Traitements", "MedecinRecommandant_Id");
            AddForeignKey("dbo.Traitements", "MedecinRecommandant_Id", "dbo.Medecins", "Id");
            AddForeignKey("dbo.Traitements", "MedecinTraitant_Id", "dbo.Medecins", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Traitements", "MedecinTraitant_Id", "dbo.Medecins");
            DropForeignKey("dbo.Traitements", "MedecinRecommandant_Id", "dbo.Medecins");
            DropIndex("dbo.Traitements", new[] { "MedecinRecommandant_Id" });
            DropIndex("dbo.Traitements", new[] { "MedecinTraitant_Id" });
            AlterColumn("dbo.Traitements", "MedecinTraitant_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Traitements", "MedecinRecommandant_Id");
            RenameColumn(table: "dbo.Traitements", name: "MedecinTraitant_Id", newName: "MedecinId");
            CreateIndex("dbo.Traitements", "MedecinId");
            AddForeignKey("dbo.Traitements", "MedecinId", "dbo.Medecins", "Id", cascadeDelete: true);
        }
    }
}
