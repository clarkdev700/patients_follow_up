namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttributPatientMiseAjour : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Patients", "Adresse", c => c.String());
            AddColumn("dbo.Patients", "LieuNaissance", c => c.String());
            AddColumn("dbo.Patients", "DateNaissance", c => c.DateTime());
            AddColumn("dbo.Patients", "Profession", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Patients", "Profession");
            DropColumn("dbo.Patients", "DateNaissance");
            DropColumn("dbo.Patients", "LieuNaissance");
            DropColumn("dbo.Patients", "Adresse");
        }
    }
}
