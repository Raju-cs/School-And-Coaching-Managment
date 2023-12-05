namespace App.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCoachingMoneyType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CoachingAddMoneyType",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 500),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        UpdatedBy = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Remarks = c.String(),
                        ChangeLog = c.String(),
                        ActivityId = c.Guid(nullable: false),
                        AddTypeFormate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CoachingAddMoneyType");
        }
    }
}
