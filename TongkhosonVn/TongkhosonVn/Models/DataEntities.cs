using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace TongkhosonVn.Models
{
    public class DataEntities:DbContext
    {
        public DataEntities() : base("DataEntities")
        {

        }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<ConfigSite> ConfigSites { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Member> Members { get; set; }  
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> Table_PostCategory { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> Table_ProductCategory { get; set; }
        public DbSet<TradeMark> TradeMarks { get; set; }
        public DbSet<TagProductCategory> Tag_ProductCategry_TradeMarks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ReceiveEmail> ReceiveEmails { get; set; }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<TypicalCustomer> TypicalCustomers { get; set; }
        public DbSet<LibraryImage> LibraryImages { get; set; }
        
        


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductCategory>().ToTable("Table_ProductCategory");
            modelBuilder.Entity<PostCategory>().ToTable("Table_PostCategory");
            modelBuilder.Entity<TagProductCategory>().ToTable("Tag_ProductCategry_TradeMarks");
        }
    }
}
