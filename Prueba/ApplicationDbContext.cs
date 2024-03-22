using Microsoft.EntityFrameworkCore;
using Prueba.Entities;

namespace Prueba;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Sorteo> Sorteos { get; set; }

    public DbSet<Cliente> Clientes { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>()
            .HasOne(x => x.Sorteo)
            .WithMany()
            .HasForeignKey(x => x.IdSorteo);

        modelBuilder.Entity<Ticket>()
            .HasOne(x => x.Cliente)
            .WithMany()
            .HasForeignKey(x => x.IdCliente);
        
        modelBuilder.Entity<Ticket>()
            .HasOne(x => x.Usuario)
            .WithMany()
            .HasForeignKey(x => x.IdUsuario);
        
        base.OnModelCreating(modelBuilder);
    }
}