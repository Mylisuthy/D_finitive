using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyTravel.Data;

namespace SkyTravel.Controllers
{
    public class TicketHistoryController : Controller
    {
        private readonly AppDbContext _context;
        public TicketHistoryController(AppDbContext context)
        {
            _context = context;
        }

        // Acción principal para listar el historial
        public async Task<IActionResult> Index()
        {
            var list = await _context.TicketHistories
                .Include(th => th.Reserva)
                .ThenInclude(r => r.pasajero)
                .Include(th => th.Reserva.vuelo)
                .OrderByDescending(th => th.Fecha)
                .ToListAsync();

            return View(list);
        }
    }
}