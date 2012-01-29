namespace Ideastrike.Nancy.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Idea_AdminResponse : DbMigration
    {
        public override void Up()
        {
            AddColumn("Ideas", "AdminResponse", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Ideas", "AdminResponse");
        }
    }
}
