namespace App.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTeacherPaymentHistory : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PaymentHistory", newName: "TeacherPaymentHistory");
            AddColumn("dbo.TeacherPaymentHistory", "TeacherId", c => c.Guid(nullable: false));
            DropColumn("dbo.TeacherPaymentHistory", "StudentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TeacherPaymentHistory", "StudentId", c => c.Guid(nullable: false));
            DropColumn("dbo.TeacherPaymentHistory", "TeacherId");
            RenameTable(name: "dbo.TeacherPaymentHistory", newName: "PaymentHistory");
        }
    }
}
