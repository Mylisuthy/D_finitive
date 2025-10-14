namespace SkyTravel.Models;

public class ReservaViewModel
{
    public int PasajeroId { get; set; }
    public int VueloId { get; set; }

    public List<Pasajero>? Pasajeros { get; set; }
    public List<Vuelo>? Vuelos { get; set; }
}