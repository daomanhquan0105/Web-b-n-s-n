namespace TongkhosonVn.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateDatabaseForAll : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BannerName = c.String(nullable: false, maxLength: 100),
                        Image = c.String(maxLength: 500),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Location = c.Int(nullable: false),
                        Url = c.String(maxLength: 500),
                        DisplayOrder = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConfigSites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 100),
                        Description = c.String(maxLength: 500),
                        HeaderCode = c.String(),
                        FooterCode = c.String(),
                        Facebook = c.String(maxLength: 500),
                        Youtube = c.String(maxLength: 500),
                        Instagram = c.String(maxLength: 500),
                        Twitter = c.String(maxLength: 500),
                        GoogleMaps = c.String(),
                        Address = c.String(maxLength: 500),
                        PhoneNumber = c.String(maxLength: 15),
                        Email = c.String(maxLength: 100),
                        LogoTop = c.String(maxLength: 500),
                        LogoBottom = c.String(maxLength: 500),
                        Payment = c.String(),
                        Contact = c.String(),
                        Helper = c.String(),
                        Transport = c.String(),
                        PricePaint = c.String(),
                        RefundPolicy = c.String(),
                        CopyRight = c.String(),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(maxLength: 60),
                        Email = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(nullable: false, maxLength: 15),
                        Subject = c.String(maxLength: 200),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FeedBacks",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        FullNname = c.String(),
                        Avatar = c.String(),
                        Content = c.String(nullable: false, storeType: "ntext"),
                        Description = c.String(nullable: false, maxLength: 500),
                        DisplayOrder = c.Int(nullable: false),
                        NoteContent = c.String(),
                        Active = c.Boolean(nullable: false),
                        FlagHome = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.LibraryImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Avatar = c.String(),
                        DisplayOrder = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        FlagHome = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 50),
                        Address = c.String(maxLength: 200),
                        Phone = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        MemberID = c.Long(nullable: false),
                        ProductID = c.Long(nullable: false),
                        CreateDate = c.DateTime(nullable: false, storeType: "date"),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        ShipDate = c.DateTime(nullable: false, storeType: "date"),
                        Status = c.Boolean(nullable: false),
                        OrderCode = c.String(maxLength: 50),
                        Remove = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.MemberID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(nullable: false, maxLength: 20),
                        Images = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OriginalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 500),
                        Detail = c.String(nullable: false),
                        Warranty = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false, storeType: "date"),
                        DisplayOrder = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        FlagHome = c.Boolean(nullable: false),
                        View = c.Int(nullable: false),
                        ProductCategoryID = c.Int(nullable: false),
                        TradeMarkID = c.Int(),
                        Url = c.String(maxLength: 500),
                        MetaTitle = c.String(maxLength: 100),
                        MetaDescription = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TradeMarks", t => t.TradeMarkID)
                .ForeignKey("dbo.Table_ProductCategory", t => t.ProductCategoryID, cascadeDelete: true)
                .Index(t => t.ProductCategoryID)
                .Index(t => t.TradeMarkID);
            
            CreateTable(
                "dbo.Table_ProductCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        DisplayOrder = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false, storeType: "date"),
                        Active = c.Boolean(nullable: false),
                        ShowMenuVertical = c.Boolean(nullable: false),
                        FlagHome = c.Boolean(nullable: false),
                        ParentProductCategoryId = c.Int(),
                        Url = c.String(maxLength: 500),
                        MetaTitle = c.String(maxLength: 100),
                        MetaDescription = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Table_ProductCategory", t => t.ParentProductCategoryId)
                .Index(t => t.ParentProductCategoryId);
            
            CreateTable(
                "dbo.Tag_ProductCategry_TradeMarks",
                c => new
                    {
                        TradeMarkID = c.Int(nullable: false),
                        ProductCategoryID = c.Int(nullable: false),
                        TagName = c.String(),
                        DisplayOrder = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.TradeMarkID, t.ProductCategoryID })
                .ForeignKey("dbo.Table_ProductCategory", t => t.ProductCategoryID, cascadeDelete: true)
                .ForeignKey("dbo.TradeMarks", t => t.TradeMarkID, cascadeDelete: true)
                .Index(t => t.TradeMarkID)
                .Index(t => t.ProductCategoryID);
            
            CreateTable(
                "dbo.TradeMarks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Body = c.String(),
                        Avatar = c.String(maxLength: 250),
                        DisplayOrder = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        FlagHome = c.Boolean(nullable: false),
                        Url = c.String(maxLength: 500),
                        MetaTitle = c.String(maxLength: 100),
                        MetaDescription = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 500),
                        Content = c.String(nullable: false),
                        Image = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        DisPlayOrder = c.Int(nullable: false),
                        View = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        FlagHome = c.Boolean(nullable: false),
                        PostCategoryID = c.Int(nullable: false),
                        Url = c.String(maxLength: 500),
                        MetaTitle = c.String(maxLength: 100),
                        MetaDescription = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Table_PostCategory", t => t.PostCategoryID, cascadeDelete: true)
                .Index(t => t.PostCategoryID);
            
            CreateTable(
                "dbo.Table_PostCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 50),
                        DisplayOrder = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        ShowMenu = c.Boolean(nullable: false),
                        ParentCategoryId = c.Int(),
                        TypeCategory = c.Int(nullable: false),
                        Url = c.String(maxLength: 500),
                        MetaTitle = c.String(maxLength: 100),
                        MetaDescription = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Table_PostCategory", t => t.ParentCategoryId)
                .Index(t => t.ParentCategoryId);
            
            CreateTable(
                "dbo.ReceiveEmails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TypicalCustomers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(maxLength: 100),
                        Avatar = c.String(maxLength: 200),
                        Description = c.String(maxLength: 500),
                        DisplayOrder = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50),
                        PassWord = c.String(nullable: false, maxLength: 120),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "PostCategoryID", "dbo.Table_PostCategory");
            DropForeignKey("dbo.Table_PostCategory", "ParentCategoryId", "dbo.Table_PostCategory");
            DropForeignKey("dbo.Orders", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "ProductCategoryID", "dbo.Table_ProductCategory");
            DropForeignKey("dbo.Table_ProductCategory", "ParentProductCategoryId", "dbo.Table_ProductCategory");
            DropForeignKey("dbo.Products", "TradeMarkID", "dbo.TradeMarks");
            DropForeignKey("dbo.Tag_ProductCategry_TradeMarks", "TradeMarkID", "dbo.TradeMarks");
            DropForeignKey("dbo.Tag_ProductCategry_TradeMarks", "ProductCategoryID", "dbo.Table_ProductCategory");
            DropForeignKey("dbo.Orders", "MemberID", "dbo.Members");
            DropIndex("dbo.Table_PostCategory", new[] { "ParentCategoryId" });
            DropIndex("dbo.Posts", new[] { "PostCategoryID" });
            DropIndex("dbo.Tag_ProductCategry_TradeMarks", new[] { "ProductCategoryID" });
            DropIndex("dbo.Tag_ProductCategry_TradeMarks", new[] { "TradeMarkID" });
            DropIndex("dbo.Table_ProductCategory", new[] { "ParentProductCategoryId" });
            DropIndex("dbo.Products", new[] { "TradeMarkID" });
            DropIndex("dbo.Products", new[] { "ProductCategoryID" });
            DropIndex("dbo.Orders", new[] { "ProductID" });
            DropIndex("dbo.Orders", new[] { "MemberID" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.TypicalCustomers");
            DropTable("dbo.ReceiveEmails");
            DropTable("dbo.Table_PostCategory");
            DropTable("dbo.Posts");
            DropTable("dbo.TradeMarks");
            DropTable("dbo.Tag_ProductCategry_TradeMarks");
            DropTable("dbo.Table_ProductCategory");
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.Members");
            DropTable("dbo.LibraryImages");
            DropTable("dbo.FeedBacks");
            DropTable("dbo.Contacts");
            DropTable("dbo.ConfigSites");
            DropTable("dbo.Banners");
        }
    }
}
