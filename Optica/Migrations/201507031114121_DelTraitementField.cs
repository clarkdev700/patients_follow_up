namespace Optica.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DelTraitementField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Traitements", "Del", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Traitements", "Del");
        }
    }
}
