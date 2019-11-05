using fashionMenApi.Models;
using Microsoft.EntityFrameworkCore;

namespace fashionMenApi.Contexts
{
    public class FashionMenDB: DbContext
    {
        public FashionMenDB(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Producto> productos { get; set; }
        public DbSet<Pedido> pedidos { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity => { entity.ToTable("usuarios"); });
            modelBuilder.Entity<Producto>(entity => { entity.ToTable("producto"); });
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("pedido");
                entity.HasKey(e => e.idPed);
                entity.Property(p => p.entregado).HasConversion<int>();
                entity.HasOne(p => p.usuario)
                    .WithMany(u => u.pedidos)
                    .HasForeignKey(p => p.id_usu);
                entity.HasOne(p => p.producto)
                    .WithMany(pr => pr.pedidos)
                    .HasForeignKey(p => p.id_producto);
            });
        }
    }
    
}