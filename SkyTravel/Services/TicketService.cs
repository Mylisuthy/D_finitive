using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using SkyTravel.Data;
using SkyTravel.Models;

public class TicketService
{
    private readonly AppDbContext _context;
    public TicketService(AppDbContext context) => _context = context;

    public async Task<(bool ok, string message, byte[]? pdf)> GenerateTicketPdfAsync(int reservaId)
    {
        var reserva = await _context.Reservas
            .Include(r => r.pasajero)
            .Include(r => r.vuelo)
            .FirstOrDefaultAsync(r => r.Id == reservaId);

        if (reserva == null)
            return (false, "Reserva no encontrada", null);

        try
        {
            Console.WriteLine($"[TicketService] Generando ticket para reserva {reservaId}...");

            // Construcción del documento
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(12));
                    page.Content().Column(col =>
                    {
                        col.Spacing(6);
                        col.Item().Text("SKYTRAVEL - TICKET DE VUELO").SemiBold().FontSize(16).AlignCenter();
                        col.Item().Text($"Pasajero: {reserva.pasajero?.Name ?? "N/A"}");
                        col.Item().Text($"Documento: {reserva.pasajero?.Document.ToString() ?? "N/A"}");
                        col.Item().Text($"Vuelo: {reserva.vuelo?.CodeVuelo ?? "N/A"}");
                        col.Item().Text($"Origen: {reserva.vuelo?.Origen ?? "N/A"} → Destino: {reserva.vuelo?.Destino ?? "N/A"}");

                        // Fecha: manejamos null-safety
                        string fechaSalida = reserva.vuelo != null
                            ? reserva.vuelo.Fsalida.ToString("dd/MM/yyyy")
                            : "N/A";
                        col.Item().Text($"Fecha de salida: {fechaSalida}");

                        col.Item().Text($"Asiento: {reserva.Asiento ?? "Sin asignar"}");
                        col.Item().Text($"Código de reserva: {reserva.CodigoReserva}");
                        col.Item().Text($"Estado del ticket: Generado");
                    });
                });
            });

            // Generar bytes del PDF
            // Dependiendo de la versión de QuestPDF la llamada puede ser GeneratePdf() o GenerateAsByteArray().
            // Usamos GeneratePdf() que es la forma recomendada en la documentación actual.
            var bytes = document.GeneratePdf();

            // Registrar historial
            var hist = new TicketHistory
            {
                ReservaId = reserva.Id,
                Estado = "Generado",
                Mensaje = "OK"
            };

            await _context.TicketHistories.AddAsync(hist);
            await _context.SaveChangesAsync();

            Console.WriteLine($"[TicketService] Ticket generado correctamente para reserva {reservaId}.");
            return (true, "Generado", bytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TicketService][ERROR] {ex.Message}");
            var hist = new TicketHistory
            {
                ReservaId = reserva.Id,
                Estado = "Error",
                Mensaje = ex.Message
            };
            await _context.TicketHistories.AddAsync(hist);
            await _context.SaveChangesAsync();
            return (false, ex.Message, null);
        }
    }
}
