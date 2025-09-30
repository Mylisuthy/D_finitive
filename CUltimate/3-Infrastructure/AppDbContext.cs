using masTer._2_Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace masTer.Data;

public class AppDbContext : DbContext
{
    //--------------------------Humanoid---------------------
    // My Super Person
    public DbSet<Person> Persons { get; set; }

    // main gate of inheritance
    public DbSet<Client> Clients { get; set; }
    public DbSet<Veterinary> Veterinaries { get; set; }
    //------------------------------------------------------

    // my business
    public DbSet<VeterinaryAppointment> VetsAppointments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql(
                "server=localhost;" +
                "database=riwi;" +
                "user=root;" +
                "password=Qwe.123*",
                new MySqlServerVersion(new Version(8, 0, 36))
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de herencia: Table Per Hierarchy (TPH)
        modelBuilder.Entity<Person>()
            .HasDiscriminator<string>("PersonType")  // Columna que indica el tipo
            .HasValue<Person>("Person")
            .HasValue<Client>("Client")
            .HasValue<Veterinary>("Veterinary");

        // Configuración opcional: limitar longitudes o requerimientos
        modelBuilder.Entity<Veterinary>()
            .Property(v => v.Speciality)
            .HasMaxLength(100);

        // Ejemplo: podrías definir reglas específicas para Client
        modelBuilder.Entity<Client>()
            .Property(c => c.Email)
            .IsRequired();
    }
}