namespace SceptrDocumentation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SupplierProducts",
                c => new
                    {
                        SupplierId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SupplierId, t.ProductId })
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.SupplierId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ClientSupplierMaps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.SupplierId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(),
                        Keyword = c.String(),
                        ProductId = c.Int(nullable: false),
                        VerbId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Verbs", t => t.VerbId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.VerbId);
            
            CreateTable(
                "dbo.Verbs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionAnswerMappers",
                c => new
                    {
                        QuestionId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => new { t.QuestionId, t.SupplierId })
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: true)
                .Index(t => t.QuestionId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.SupplierClients",
                c => new
                    {
                        Supplier_ID = c.Int(nullable: false),
                        Client_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Supplier_ID, t.Client_ID })
                .ForeignKey("dbo.Suppliers", t => t.Supplier_ID, cascadeDelete: true)
                .ForeignKey("dbo.Clients", t => t.Client_ID, cascadeDelete: true)
                .Index(t => t.Supplier_ID)
                .Index(t => t.Client_ID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.SupplierClients", new[] { "Client_ID" });
            DropIndex("dbo.SupplierClients", new[] { "Supplier_ID" });
            DropIndex("dbo.QuestionAnswerMappers", new[] { "SupplierId" });
            DropIndex("dbo.QuestionAnswerMappers", new[] { "QuestionId" });
            DropIndex("dbo.Questions", new[] { "VerbId" });
            DropIndex("dbo.Questions", new[] { "ProductId" });
            DropIndex("dbo.ClientSupplierMaps", new[] { "ProductId" });
            DropIndex("dbo.ClientSupplierMaps", new[] { "SupplierId" });
            DropIndex("dbo.ClientSupplierMaps", new[] { "ClientId" });
            DropIndex("dbo.SupplierProducts", new[] { "ProductId" });
            DropIndex("dbo.SupplierProducts", new[] { "SupplierId" });
            DropForeignKey("dbo.SupplierClients", "Client_ID", "dbo.Clients");
            DropForeignKey("dbo.SupplierClients", "Supplier_ID", "dbo.Suppliers");
            DropForeignKey("dbo.QuestionAnswerMappers", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.QuestionAnswerMappers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.Questions", "VerbId", "dbo.Verbs");
            DropForeignKey("dbo.Questions", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ClientSupplierMaps", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ClientSupplierMaps", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.ClientSupplierMaps", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.SupplierProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.SupplierProducts", "SupplierId", "dbo.Suppliers");
            DropTable("dbo.SupplierClients");
            DropTable("dbo.QuestionAnswerMappers");
            DropTable("dbo.Verbs");
            DropTable("dbo.Questions");
            DropTable("dbo.ClientSupplierMaps");
            DropTable("dbo.Products");
            DropTable("dbo.SupplierProducts");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Clients");
        }
    }
}
