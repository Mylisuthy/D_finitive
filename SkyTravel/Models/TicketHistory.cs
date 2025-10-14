namespace SkyTravel.Models;

public class TicketHistory
{
    public int Id { get; set; }
    public int ReservaId { get; set; }
    public Reserva Reserva { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
    public string Estado { get; set; } = "Generado";
    public string? Mensaje { get; set; }
}