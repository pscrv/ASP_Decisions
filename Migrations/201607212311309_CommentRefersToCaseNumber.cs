namespace ASP_Decisions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentRefersToCaseNumber : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "Decision_Id", "dbo.Decisions");
            DropIndex("dbo.Comments", new[] { "Decision_Id" });
            AddColumn("dbo.Comments", "CaseNumber", c => c.String(nullable: false));
            DropColumn("dbo.Comments", "Decision_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "Decision_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Comments", "CaseNumber");
            CreateIndex("dbo.Comments", "Decision_Id");
            AddForeignKey("dbo.Comments", "Decision_Id", "dbo.Decisions", "Id", cascadeDelete: true);
        }
    }
}
