using Inventory.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Tienda> Tiendas => Set<Tienda>();
    public DbSet<Articulo> Articulos => Set<Articulo>();
    public DbSet<ArticuloTienda> ArticuloTiendas => Set<ArticuloTienda>();
    public DbSet<ClienteArticulo> ClienteArticulos => Set<ClienteArticulo>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Cliente>(e =>
        {
            e.Property(x => x.Nombre).HasMaxLength(80).IsRequired();
            e.Property(x => x.Apellidos).HasMaxLength(120).IsRequired();
            e.HasIndex(x => new { x.Apellidos, x.Nombre });
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Email).HasMaxLength(256);
        });

        b.Entity<Tienda>(e =>
        {
            e.Property(x => x.Sucursal).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.Sucursal).IsUnique();
        });

        b.Entity<Tienda>().HasData(
            new Tienda { Id = 1, Sucursal = "Tienda Centro", Direccion = "Av. Principal #123, CDMX" },
            new Tienda { Id = 2, Sucursal = "Tienda Norte",  Direccion = "Calle Secundaria #45, CDMX" }
        );

        b.Entity<Articulo>(e =>
        {
            e.Property(x => x.Codigo).HasMaxLength(50).IsRequired();
            e.Property(x => x.Descripcion).HasMaxLength(200).IsRequired();
            e.Property(x => x.Precio).HasPrecision(18,2);
            e.HasIndex(x => x.Codigo).IsUnique();
            e.HasCheckConstraint("CK_Articulos_Precio", "[Precio] >= 0");
        });

        b.Entity<Articulo>().HasData(
                new Articulo { Id = 1, Codigo = "A1001", Descripcion = "Camiseta básica blanca", Precio = 199.99m, ImagenUrl = "https://picsum.photos/seed/a1001/600/400" },
                new Articulo { Id = 2, Codigo = "A1002", Descripcion = "Pantalón de mezclilla",   Precio = 499.50m, ImagenUrl = "https://picsum.photos/seed/a1002/600/400" },
                new Articulo { Id = 3, Codigo = "A1003", Descripcion = "Tenis deportivos",       Precio = 899.00m, ImagenUrl = "https://picsum.photos/seed/a1003/600/400" },
                new Articulo { Id = 4, Codigo = "A1004", Descripcion = "Sudadera con capucha",   Precio = 650.00m, ImagenUrl = "https://picsum.photos/seed/a1004/600/400" },
                new Articulo { Id = 5, Codigo = "A1005", Descripcion = "Reloj casual",           Precio = 1200.00m, ImagenUrl = "https://picsum.photos/seed/a1005/600/400" },
                new Articulo { Id = 6, Codigo = "A1006", Descripcion = "Mochila escolar",        Precio = 350.00m, ImagenUrl = "https://picsum.photos/seed/a1006/600/400" },
                new Articulo { Id = 7, Codigo = "A1007", Descripcion = "Gorra clásica",          Precio = 150.00m, ImagenUrl = "https://picsum.photos/seed/a1007/600/400" },
                new Articulo { Id = 8, Codigo = "A1008", Descripcion = "Cinturón de piel",       Precio = 275.00m, ImagenUrl = "https://picsum.photos/seed/a1008/600/400" }
            );

        b.Entity<ArticuloTienda>(e =>
        {
            e.HasKey(x => new { x.ArticuloId, x.TiendaId });
            e.Property(x => x.FechaAlta)
                   .HasColumnType("datetime2")
                   .HasDefaultValueSql("GETUTCDATE()");

            e.HasOne(x => x.Articulo)
             .WithMany(a => a.ArticuloTiendas)
             .HasForeignKey(x => x.ArticuloId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Tienda)
             .WithMany(t => t.ArticuloTiendas)
             .HasForeignKey(x => x.TiendaId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        var alta = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        b.Entity<ArticuloTienda>().HasData(
            new ArticuloTienda { ArticuloId = 1, TiendaId = 1, FechaAlta = alta, Stock = 50 },
            new ArticuloTienda { ArticuloId = 2, TiendaId = 1, FechaAlta = alta, Stock = 30 },
            new ArticuloTienda { ArticuloId = 3, TiendaId = 1, FechaAlta = alta, Stock = 20 },
            new ArticuloTienda { ArticuloId = 4, TiendaId = 1, FechaAlta = alta, Stock = 15 },

            new ArticuloTienda { ArticuloId = 5, TiendaId = 2, FechaAlta = alta, Stock = 40 },
            new ArticuloTienda { ArticuloId = 6, TiendaId = 2, FechaAlta = alta, Stock = 25 },
            new ArticuloTienda { ArticuloId = 7, TiendaId = 2, FechaAlta = alta, Stock = 60 },
            new ArticuloTienda { ArticuloId = 8, TiendaId = 2, FechaAlta = alta, Stock = 10 }
        );

        b.Entity<ClienteArticulo>(e =>
        {
            e.HasKey(x => new { x.ClienteId, x.ArticuloId, x.Fecha });
            e.Property(x => x.Fecha).HasDefaultValueSql("SYSUTCDATETIME()");

            e.HasOne(x => x.Cliente)
             .WithMany(c => c.ClienteArticulos)
             .HasForeignKey(x => x.ClienteId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Articulo)
             .WithMany(a => a.ClienteArticulos)
             .HasForeignKey(x => x.ArticuloId)
             .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
