namespace ASP_Decisions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        DateSubmitted = c.DateTime(nullable: false),
                        DatePublished = c.DateTime(),
                        IsAccepted = c.Boolean(nullable: false),
                        IsChecked = c.Boolean(nullable: false),
                        Author_Id = c.String(nullable: false, maxLength: 128),
                        Decision_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id, cascadeDelete: true)
                .ForeignKey("dbo.Decisions", t => t.Decision_Id, cascadeDelete: true)
                .Index(t => t.Author_Id)
                .Index(t => t.Decision_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Decision_Id", "dbo.Decisions");
            DropForeignKey("dbo.Comments", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "Decision_Id" });
            DropIndex("dbo.Comments", new[] { "Author_Id" });
            DropTable("dbo.Comments");
        }
    }
}
