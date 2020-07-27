namespace Lessons_Cordinator_beta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MIG1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.dayInformations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        studentID = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        absent = c.Boolean(nullable: false),
                        mark = c.Double(nullable: false),
                        note = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        hour = c.Int(nullable: false),
                        minutes = c.Int(nullable: false),
                        day = c.Int(nullable: false),
                        gender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        phone1 = c.String(),
                        phone2 = c.String(),
                        whatsAppNumber = c.String(),
                        groupID = c.Int(nullable: false),
                        school = c.String(),
                        address = c.String(),
                        gender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Students");
            DropTable("dbo.Groups");
            DropTable("dbo.dayInformations");
        }
    }
}
