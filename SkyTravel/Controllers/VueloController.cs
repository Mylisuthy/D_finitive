using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyTravel.Data;
using SkyTravel.Interfaces;
using SkyTravel.Models;

namespace SkyTravel.Controllers;

public class VueloController : Controller, ICrud<Vuelo>
{
    private readonly AppDbContext _context;

    public VueloController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var vuelos = await _context.Vuelos.ToListAsync();
        if (vuelos == null || !vuelos.Any())
        {
            TempData["Info"] = "No hay vuelos registrados todavía.";
        }
        return View(vuelos);
    }
    
    public async Task<IActionResult> Register(Vuelo entity)
    {
        try
        {
            var esObligatorio = new Dictionary<string, string>
            {
                { "CodeVuelo", entity.CodeVuelo },
                { "Origen", entity.Origen },
                { "Destino", entity.Destino }
            };
        
            foreach (var campo in esObligatorio)
            {
                if (string.IsNullOrWhiteSpace(campo.Value))
                {
                    TempData["Error"] = $"recuerda que el '{campo.Key}' es obligatorio.";
                    return RedirectToAction(nameof(ListAll));
                }
            }

            var normalizedCode = entity.CodeVuelo.Trim().ToUpperInvariant();
        
            entity.CodeVuelo = normalizedCode;

            if (await Exist(normalizedCode))
            {
                TempData["Error"] = $"El codigo {normalizedCode} ya esta registrado";
                return RedirectToAction(nameof(ListAll));
            }

            entity.CodeVuelo = normalizedCode;
            entity.InitialCapacity = entity.Capacity;
            entity.Estado = EstadoVuelo.Programado.ToString();
            await _context.Vuelos.AddAsync(entity);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Vuelo registrado correctamente";
            return RedirectToAction(nameof(ListAll));
        }
        catch (DbUpdateException ex)
        {
            TempData["Error"] = "Error al guardar en la base de datos. Verifica los datos ingresados.";
            Console.WriteLine($"[DB ERROR] {ex.InnerException?.Message ?? ex.Message}");
            return RedirectToAction(nameof(ListAll));
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ocurrió un error inesperado: " + ex.Message;
            Console.WriteLine($"[ERROR] {ex.Message}");
            return RedirectToAction(nameof(ListAll));
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var vuelo = await _context.Vuelos.FindAsync(id);
            if (vuelo == null)
            {
                TempData["Error"] = "Vuelo no encontrado.";
            }
            else
            {
                _context.Vuelos.Remove(vuelo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Vuelo eliminado correctamente.";
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al eliminar: " + ex.Message;
        }

        var vuelos = await _context.Vuelos.ToListAsync();
        return View("Index", vuelos);
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var vuelo = await _context.Vuelos.FindAsync(id);
        if (vuelo == null)
            return NotFound();

        return PartialView("VueloUpdate", vuelo);;
    }

    [HttpPost]
    public async Task<IActionResult> Update(Vuelo entity)
    {
        try
        {
            if (!ModelState.IsValid)
                return View("VueloUpdate", entity);

            var vuelo = await _context.Vuelos.FindAsync(entity.Id);
            if (vuelo == null)
            {
                TempData["Error"] = "Vuelo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            vuelo.Origen = entity.Origen;
            vuelo.Destino = entity.Destino;
            vuelo.Capacity = entity.Capacity;
            vuelo.CodeVuelo = entity.CodeVuelo;
            vuelo.Fsalida = entity.Fsalida;
            vuelo.Fllegada = entity.Fllegada;

            _context.Vuelos.Update(vuelo);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Vuelo actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            TempData["Error"] = "Error al actualizar los datos. Verifica los campos ingresados.";
            Console.WriteLine($"[DB UPDATE ERROR] {ex.InnerException?.Message ?? ex.Message}");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ocurrió un error inesperado: " + ex.Message;
            Console.WriteLine($"[ERROR] {ex.Message}");
            return RedirectToAction(nameof(Index));
        }
    }
    
    public async Task<IActionResult> ListAll()
    {
        List<Vuelo> vuelos = await _context.Vuelos.ToListAsync();
        return View("Index", vuelos);
    }
    
    public async Task<bool> Exist(string codeVuelo)
    {
        if (string.IsNullOrWhiteSpace(codeVuelo)) return false;
        var normalized = codeVuelo.Trim().ToUpperInvariant();
        return await _context.Vuelos.AnyAsync(cv => cv.CodeVuelo
            .ToUpper() == normalized);
    }
    
    [HttpPost]
    public async Task<IActionResult> ChangeState(int id, string nuevoEstado)
    {
        var vuelo = await _context.Vuelos.FindAsync(id);
        if (vuelo == null) { TempData["Error"] = "Vuelo no encontrado."; return RedirectToAction(nameof(Index)); }

        vuelo.Estado = nuevoEstado;
        _context.Vuelos.Update(vuelo);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Estado actualizado.";
        return RedirectToAction(nameof(Index));
    }
}