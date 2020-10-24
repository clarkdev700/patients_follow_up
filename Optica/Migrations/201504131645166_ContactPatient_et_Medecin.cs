namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContactPatient_et_Medecin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Medecins", "Contact", c => c.String());
            AddColumn("dbo.Patients", "Contact", c => c.String());
            AddColumn("dbo.Patients", "Email", c => c.String());
            AlterColumn("dbo.ControlePats", "DateEffectuerControle", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ControlePats", "DateEffectuerControle", c => c.DateTime(nullable: false));
            DropColumn("dbo.Patients", "Email");
            DropColumn("dbo.Patients", "Contact");
            DropColumn("dbo.Medecins", "Contact");
        }
    }
}
