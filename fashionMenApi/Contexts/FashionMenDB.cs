using fashionMenApi.Models;
using Microsoft.EntityFrameworkCore;

namespace fashionMenApi.Contexts
{
    public class FashionMenDB: DbContext
    {
        public FashionMenDB(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<User> users { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Order> orders { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => { entity.ToTable("users"); });
            modelBuilder.Entity<Product>(entity => { entity.ToTable("products"); });
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.HasKey(e => e.id);
                entity.Property(p => p.shipped).HasConversion<int>();
                entity.HasOne(p => p.user)
                    .WithMany(u => u.orders)
                    .HasForeignKey(p => p.user_id);
                
                entity.HasOne(p => p.product)
                    .WithMany(pr => pr.orders)
                    .HasForeignKey(p => p.product_id);
            });
        }
    }
    
}