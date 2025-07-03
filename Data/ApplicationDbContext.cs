using MechantInventory.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace MechantInventory.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions option) : base(option) { }
   
        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CommunicationLog> CommunicationLogs { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(
             new Product
             {
                 ProductId = 1,
                 Name = "Iwatch Series 5",
                 ImageUrl = "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691699385/watch-prod-3_mvvw3x.webp",
                 Price = 180000.00m,
                 Category = "Watch",

             }, new Product
             {
                 ProductId = 2,
                 Name = "Iphone 15 Promax",
                 ImageUrl = "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691699516/iphone_14_purple_ck04dl.png",
                 Price = 1400000.00m,
                 Category = "Phone",

             }, new Product
             {
                 ProductId = 3,
                 Name = "Iphone 14 Pro",
        
                 ImageUrl = "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691669759/iphone12_fid8e0.png",
                 Price = 900000.00m,
                 Category = "Phone",

             }, new Product
             {
                 ProductId = 4,
                 Name = "Boom Headset",
                 
                 ImageUrl = "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691699439/headphone-prod-2_bmgfov.webp",
                 Price = 130000.00m,
                 Category = "Headset",

             }, new Product
             {
                 ProductId = 5,
                 Name = "Oraimo Airpod",
                 ImageUrl = "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691871784/earbuds-prod-4_la7u7j.webp",
                 Price = 34000.00m,
                 Category = "Airpod",

             }, new Product
             {
                 ProductId = 6,
                 Name = "Iphone 13 Pro",
                
                 ImageUrl = "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691871777/13pro_blue_ws4pjl.png",
                 Price = 650000.00m,
                 Category = "Phone",

             }, new Product
             {
                 ProductId = 7,
                 Name = "Samsung Watch-7",
                
                 ImageUrl = "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691871415/watch-prod-2_eya0ym.webp",
                 Price = 280000.00m,
                 Category = "Watch",

             }, new Product
             {
                 ProductId = 8,
                 Name = "Zolum Headset",
                 
                 ImageUrl = "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1691699650/headphone-prod-5_ayglse.webp",
                 Price = 125000.00m,
                 Category = "Headset",

             }, new Product
             {
                 ProductId = 10,
                 Name = "Macbook Pro 2022",
                 
                 ImageUrl = "https://res.cloudinary.com/dgsjzsrw4/image/upload/v1685574593/Mabook_Pro_aicpdz.png",
                 Price = 2155000.00m,
                 Category = "Macbook",
             }

             );
           
        }
    }
}