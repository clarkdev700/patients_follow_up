namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MiseAjourAddColummnTitreToPatient : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Traitements", "MedecinRecommandantId", "dbo.Medecins");
            DropIndex("dbo.Traitements", new[] { "MedecinRecommandantId" });
            AddColumn("dbo.Patients", "Titre", c => c.Int(nullable: false));
            AlterColumn("dbo.Traitements", "MedecinRecommandantId", c => c.Int());
            CreateIndex("dbo.Traitements", "MedecinRecommandantId");
            AddForeignKey("dbo.Traitements", "MedecinRecommandantId", "dbo.Medecins", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Traitements", "MedecinRecommandantId", "dbo.Medecins");
            DropIndex("dbo.Traitements", new[] { "MedecinRecommandantId" });
            AlterColumn("dbo.Traitements", "MedecinRecommandantId", c => c.Int(nullable: false));
            DropColumn("dbo.Patients", "Titre");
            CreateIndex("dbo.Traitements", "MedecinRecommandantId");
            AddForeignKey("dbo.Traitements", "MedecinRecommandantId", "dbo.Medecins", "Id", cascadeDelete: true);
        }
    }
}
