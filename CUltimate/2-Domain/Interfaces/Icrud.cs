using masTer._2_Domain.Models;

namespace masTer.Interfaces
{
    public interface ICrud<T> where T : Person
    {
        Task RegisterAsync(T entity);
        Task<List<T>> ListAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task EditAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}