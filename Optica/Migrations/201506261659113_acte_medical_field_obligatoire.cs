namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class acte_medical_field_obligatoire : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ActeMedicals", "Designation", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ActeMedicals", "Designation", c => c.String());
        }
    }
}
