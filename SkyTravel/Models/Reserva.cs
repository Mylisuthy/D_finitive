namespace SkyTravel.Models;

public class Reserva
{
    public int Id { get; set; }
    public DateOnly Freserva { get; set; }
    public int VueloId { get; set; }
    public Vuelo vuelo { get; set; }
    
    public int PasajeroId { get; set; }
    public Pasajero pasajero { get; set; }
}