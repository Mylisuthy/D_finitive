namespace SkyTravel.Models;

public class ReservaIndexViewModel
{
    public List<Reserva> Reservas { get; set; } = new();
    public List<Pasajero> Pasajeros { get; set; } = new();
    public List<Vuelo> Vuelos { get; set; } = new();
}