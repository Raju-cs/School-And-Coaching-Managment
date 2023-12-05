namespace App.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addmoneytable1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CoachingAcAddMoney",
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
                        Amount = c.Double(nullable: false),
                        TypeId = c.Guid(nullable: false),
                        AddMoneyDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CoachingAcAddMoney");
        }
    }
}
