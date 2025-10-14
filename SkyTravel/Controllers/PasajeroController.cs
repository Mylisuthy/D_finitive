using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyTravel.Data;
using SkyTravel.Interfaces;
using SkyTravel.Models;

namespace SkyTravel.Controllers;

public class PasajeroController : Controller, ICrud<Pasajero>
{
    private readonly AppDbContext _context;

    public PasajeroController(AppDbContext context)
    {
        _context = context;
    }

    // ✅ Listar todos los pasajeros
    public async Task<IActionResult> Index()
    {
        var pasajeros = await _context.Pasajeros.ToListAsync();
        if (pasajeros == null || !pasajeros.Any())
        {
            TempData["Info"] = "No hay pasajeros registrados todavía.";
        }
        return View(pasajeros);
    }
    
    
    public async Task<IActionResult> Register(Pasajero entity)
    {
        try
        {
            // Validaciones manuales
            var esObligatorio = new Dictionary<string, object>
            {
                { "Name", entity.Name },
                { "Document", entity.Document},
                { "Tell", entity.Tell},
                { "Email", entity.Email }
            };

            foreach (var campo in esObligatorio)
            {
                if (campo.Value == null || string.IsNullOrWhiteSpace(campo.Value.ToString()))
                {
                    TempData["Error"] = $"El campo '{campo.Key}' es obligatorio.";
                    return RedirectToAction(nameof(ListAll));
                }
            }

            // Verificar duplicado de documento
            if (await Exist(entity.Document))
            {
                TempData["Error"] = $"El documento {entity.Document} ya está registrado.";
                return RedirectToAction(nameof(ListAll));
            }

            await _context.Pasajeros.AddAsync(entity);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Pasajero registrado correctamente.";
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

    // ✅ Eliminar pasajero
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var pasajero = await _context.Pasajeros.FindAsync(id);
            if (pasajero == null)
            {
                TempData["Error"] = "Pasajero no encontrado.";
            }
            else
            {
                _context.Pasajeros.Remove(pasajero);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Pasajero eliminado correctamente.";
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error al eliminar: " + ex.Message;
        }

        var pasajeros = await _context.Pasajeros.ToListAsync();
        return View("Index", pasajeros);
    }

    // ✅ Obtener vista de actualización (modal)
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var pasajero = await _context.Pasajeros.FindAsync(id);
        if (pasajero == null)
            return NotFound();

        return PartialView("PasajeroUpdate", pasajero);
    }

    // ✅ Actualizar pasajero
    [HttpPost]
    public async Task<IActionResult> Update(Pasajero entity)
    {
        try
        {
            if (!ModelState.IsValid)
                return View("PasajeroUpdate", entity);

            var pasajero = await _context.Pasajeros.FindAsync(entity.Id);
            if (pasajero == null)
            {
                TempData["Error"] = "Pasajero no encontrado.";
                return RedirectToAction(nameof(Index));
            }
            
            var existeOtro = await _context.Pasajeros.AnyAsync(p => p.Document == entity.Document && p.Id != entity.Id);
            if (existeOtro) { TempData["Error"]="Documento ya registrado por otro pasajero."; return RedirectToAction(nameof(Index)); }

            pasajero.Name = entity.Name;
            pasajero.Document = entity.Document;
            pasajero.Tell = entity.Tell;
            pasajero.Email = entity.Email;

            _context.Pasajeros.Update(pasajero);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Pasajero actualizado correctamente.";
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

    // ✅ Listar todos (reutilizado)
    public async Task<IActionResult> ListAll()
    {
        List<Pasajero> pasajeros = await _context.Pasajeros.ToListAsync();
        return View("Index", pasajeros);
    }

    
    public async Task<bool> Exist(long document)
    {
        return await _context.Pasajeros.AnyAsync(p => p.Document == document);
    }
}
