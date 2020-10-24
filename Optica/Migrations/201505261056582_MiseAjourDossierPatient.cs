namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MiseAjourDossierPatient : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DossierPats", "NumeroDossier", c => c.String());
            DropColumn("dbo.DossierPats", "Motif");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DossierPats", "Motif", c => c.String());
            DropColumn("dbo.DossierPats", "NumeroDossier");
        }
    }
}
