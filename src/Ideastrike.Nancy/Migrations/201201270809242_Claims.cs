namespace Ideastrike.Nancy.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Claims : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "UserClaims",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        ClaimId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ClaimId })
                .ForeignKey("Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("Claims", t => t.ClaimId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ClaimId);
            
            CreateTable(
                "Claims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("UserClaims", new[] { "ClaimId" });
            DropIndex("UserClaims", new[] { "UserId" });
            DropForeignKey("UserClaims", "ClaimId", "Claims");
            DropForeignKey("UserClaims", "UserId", "Users");
            DropTable("Claims");
            DropTable("UserClaims");
        }
    }
}
