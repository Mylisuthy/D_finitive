namespace SkyTravel.Models;

public class Vuelo
{
    public int Id { get; set; }
    public string? Origen { get; set; }
    public string? Destino { get; set; }
    public DateOnly Fsalida { get; set; }
    public DateOnly Fllegada { get; set; }

    // Capacity seguirá siendo el número disponible actualmente
    public int Capacity { get; set; }

    // Guardamos la capacidad inicial para poder asignar asientos secuenciales
    public int InitialCapacity { get; set; }

    public string? CodeVuelo { get; set; }

    // Nuevo estado del vuelo
    public string Estado { get; set; } = EstadoVuelo.Programado.ToString();

    public List<Reserva> Reservas { get; set; } = new List<Reserva>();
}
public enum EstadoVuelo { Programado, EnVuelo, Finalizado, Cancelado }