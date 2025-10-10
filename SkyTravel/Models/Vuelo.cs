namespace SkyTravel.Models;

public class Vuelo
{
    public int Id { get; set; }
    public string Origen { get; set; }
    public string Destino { get; set; }
    public DateOnly Fsalida { get; set; }
    public DateOnly Fllegada { get; set; }
    public int Capacity { get; set; }
    public string CodeVuelo { get; set; }

    public List<Reserva> Reservas { get; set; } = new List<Reserva>();
}