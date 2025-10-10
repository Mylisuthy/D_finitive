using Microsoft.AspNetCore.Mvc;

namespace SkyTravel.Interfaces;

public interface ICrud<T> where T : class
{
    Task<IActionResult> Register(T entity);
    Task<IActionResult> Delete(int id);
    Task<IActionResult> Update(int id);
    Task<IActionResult> ListAll();
}