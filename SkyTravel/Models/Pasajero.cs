namespace SkyTravel.Models;

public class Pasajero
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Document { get; set; }
    public int Tell { get; set; }
    public string Email { get; set; }

    public List<Reserva> Reservas { get; set; } = new List<Reserva>();
}