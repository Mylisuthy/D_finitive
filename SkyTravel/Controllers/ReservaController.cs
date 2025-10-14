using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyTravel.Data;
using SkyTravel.Models;

namespace SkyTravel.Controllers;

public class ReservaController : Controller
{
    private readonly AppDbContext _context;
    private readonly TicketService _ticketService;

    public ReservaController(AppDbContext context, TicketService ticketService)
    {
        _context = context;
        _ticketService = ticketService;
    }
    
    public async Task<IActionResult> Ticket(int id)
    {
        var (ok, message, pdf) = await _ticketService.GenerateTicketPdfAsync(id);
        if (!ok)
        {
            TempData["Error"] = "Error al generar ticket: " + message;
            return RedirectToAction(nameof(Index));
        }
        return File(pdf!, "application/pdf", $"ticket_{id}.pdf");
    }

    // 📋 Muestra todas las reservas
    public IActionResult Index()
    {
        var model = new ReservaIndexViewModel
        {
            Reservas = _context.Reservas.Include(r => r.pasajero).Include(r => r.vuelo).ToList(),
            Pasajeros = _context.Pasajeros.ToList(),
            Vuelos = _context.Vuelos.ToList()
        };
        return View(model);
    }

    // ✈️ Mostrar formulario de reserva
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new ReservaViewModel
        {
            Pasajeros = await _context.Pasajeros.ToListAsync(),
            Vuelos = await _context.Vuelos.ToListAsync()
        };

        return View(model);
    }

    // 💾 Confirmar reserva
    [HttpPost]
    public async Task<IActionResult> Create(Reserva entity)
    {
        // valida selects
        if (entity.VueloId == 0 || entity.PasajeroId == 0)
        {
            TempData["Error"] = "Selecciona vuelo y pasajero.";
            return RedirectToAction(nameof(Create));
        }

        var vuelo = await _context.Vuelos.FindAsync(entity.VueloId);
        var pasajero = await _context.Pasajeros.FindAsync(entity.PasajeroId);

        if (vuelo == null || pasajero == null)
        {
            TempData["Error"] = "Vuelo o pasajero inválido.";
            return RedirectToAction(nameof(Create));
        }

        // Validar fecha <= 30 días
        var hoy = DateOnly.FromDateTime(DateTime.Now);
        var limite = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
        if (vuelo.Fsalida < hoy)
        {
            TempData["Error"] = "No puedes reservar un vuelo ya pasado.";
            return RedirectToAction(nameof(Create));
        }
        if (vuelo.Fsalida > limite)
        {
            TempData["Error"] = "La reserva no puede exceder 30 días de anticipación.";
            return RedirectToAction(nameof(Create));
        }

        // Verificar duplicado (reserva activa)
        bool yaReservado = await _context.Reservas.AnyAsync(r =>
            r.VueloId == vuelo.Id &&
            r.PasajeroId == pasajero.Id &&
            r.Estado == "Activa");
        if (yaReservado)
        {
            TempData["Error"] = "El pasajero ya tiene una reserva activa para este vuelo.";
            return RedirectToAction(nameof(Create));
        }

        // Cupos disponibles
        if (vuelo.Capacity <= 0)
        {
            TempData["Error"] = "No hay asientos disponibles.";
            return RedirectToAction(nameof(Create));
        }

        // Asignar asiento (número secuencial simple)
        var usedCount = vuelo.InitialCapacity - vuelo.Capacity; // cuántos ya asignados
        var seatNumber = usedCount + 1;
        entity.Asiento = seatNumber.ToString(); // puedes mejorar formato "12A" si quieres
        entity.Estado = "Activa";
        entity.Freserva = DateOnly.FromDateTime(DateTime.Now);

        // Actualizar capacidad
        vuelo.Capacity -= 1;

        try
        {
            await _context.Reservas.AddAsync(entity);
            _context.Vuelos.Update(vuelo);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Reserva confirmada correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            TempData["Error"] = "Error al guardar la reserva. Verifica los datos.";
            Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
            return RedirectToAction(nameof(Create));
        }
    }

    // ❌ Cancelar reserva
    public async Task<IActionResult> Cancelar(int id)
    {
        var reserva = await _context.Reservas
            .Include(r => r.vuelo)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reserva == null)
        {
            TempData["Error"] = "Reserva no encontrada.";
            return RedirectToAction(nameof(Index));
        }

        reserva.Estado = "Cancelada";
        reserva.vuelo.Capacity += 1;

        _context.Reservas.Update(reserva);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Reserva cancelada y asiento liberado.";
        return RedirectToAction(nameof(Index));
    }

    // 🏁 Marcar como completada
    public async Task<IActionResult> Completar(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
        {
            TempData["Error"] = "Reserva no encontrada.";
            return RedirectToAction(nameof(Index));
        }

        reserva.Estado = "Completada";
        _context.Reservas.Update(reserva);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Reserva marcada como completada.";
        return RedirectToAction(nameof(Index));
    }
}
