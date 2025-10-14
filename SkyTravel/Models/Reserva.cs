namespace SkyTravel.Models;

public class Reserva
{
    public int Id { get; set; }
    public DateOnly Freserva { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public int VueloId { get; set; }
    public Vuelo? vuelo { get; set; }
    
    public int PasajeroId { get; set; }
    public Pasajero? pasajero { get; set; }
    
    public string Estado { get; set; } = "Activa";
    public string CodigoReserva { get; set; } = Guid.NewGuid().ToString().Substring(0, 8);

    // Nuevo: asiento asignado (ej. "12A" o "12")
    public string? Asiento { get; set; }
}