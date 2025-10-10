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
        if (vuelos == null) return NotFound();
        return View(vuelos);
    }
    
    public async Task<IActionResult> Register(Vuelo entity)
    {
        if (string.IsNullOrWhiteSpace(entity.CodeVuelo))
        {
            TempData["Error"] = "El codigo del Vuelo es obligatorio";
            return RedirectToAction(nameof(ListAll));
        }
        
        var normalizedCode = entity.CodeVuelo.Trim();
        entity.CodeVuelo = normalizedCode;
        
        if (await Exist(normalizedCode))
        {
            //dato temporal con alerta
            TempData["Error"] = $"El codigo {normalizedCode} ya esta registrado";
            return RedirectToAction(nameof(ListAll));
        }
        
        await _context.Vuelos.AddAsync(entity);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Vuelo registrado correctamente";
        return RedirectToAction(nameof(ListAll));
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
                await _context.Vuelos.ExecuteDeleteAsync();
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

    public Task<IActionResult> Update(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> ListAll()
    {
        List<Vuelo> vuelos = await _context.Vuelos.ToListAsync();
        return View("Index", vuelos);
    }
    
    public async Task<bool> Exist(string CodeVuelo)
    {
        if (string.IsNullOrWhiteSpace(CodeVuelo)) return false;
        var normalized = CodeVuelo.Trim().ToLower();
        return await _context.Vuelos.AnyAsync(cv => cv.CodeVuelo.ToLower() == normalized);
    }
    
    public async Task<IActionResult> GetById(int id)
    {
        var vuelo = await _context.Vuelos.FindAsync(id);
        if (vuelo == null) return NotFound();
        return RedirectToAction(vuelo);
    }
}