namespace Ideastrike.Nancy.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AdminActivity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Activities", "OldStatus_Id", "Status");
            DropForeignKey("Activities", "NewStatus_Id", "Status");
            DropIndex("Activities", new[] { "OldStatus_Id" });
            DropIndex("Activities", new[] { "NewStatus_Id" });
            AddColumn("Activities", "OldStatus", c => c.String());
            AddColumn("Activities", "NewStatus", c => c.String());
            DropColumn("Activities", "OldStatus_Id");
            DropColumn("Activities", "NewStatus_Id");
            DropTable("Status");
        }
        
        public override void Down()
        {
            CreateTable(
                "Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("Activities", "NewStatus_Id", c => c.Int());
            AddColumn("Activities", "OldStatus_Id", c => c.Int());
            DropColumn("Activities", "NewStatus");
            DropColumn("Activities", "OldStatus");
            CreateIndex("Activities", "NewStatus_Id");
            CreateIndex("Activities", "OldStatus_Id");
            AddForeignKey("Activities", "NewStatus_Id", "Status", "Id");
            AddForeignKey("Activities", "OldStatus_Id", "Status", "Id");
        }
    }
}
