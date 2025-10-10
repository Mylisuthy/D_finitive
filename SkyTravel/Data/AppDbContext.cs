using Microsoft.EntityFrameworkCore;
using SkyTravel.Models;

namespace SkyTravel.Data;

public class AppDbContext : DbContext
{
    public DbSet<Vuelo> Vuelos { get; set; }
    public DbSet<Pasajero> Pasajeros { get; set; }
    public DbSet<Reserva> Reservas { get; set; }

    public AppDbContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vuelo>()
            .HasMany(r => r.Reservas)
            .WithOne(v => v.vuelo)
            .HasForeignKey(v => v.VueloId);
        
        modelBuilder.Entity<Pasajero>()
            .HasMany(r => r.Reservas)
            .WithOne(p => p.pasajero)
            .HasForeignKey(p => p.PasajeroId);
    }
}