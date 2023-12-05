namespace App.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CoachingMoneyWidthdrawHistory",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Remarks = c.String(),
                        ChangeLog = c.String(),
                        ActivityId = c.Guid(nullable: false),
                        PeriodId = c.Guid(nullable: false),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CoachingMoneyWidthdrawHistory");
        }
    }
}
