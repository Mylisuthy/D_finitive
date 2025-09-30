using masTer._2_Domain.Models;
using masTer.Data;
using masTer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace masTer.Services
{
    public abstract class GenericMet<T> : ICrud<T> where T : Person
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        protected GenericMet(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task RegisterAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "El registro no puede ser nulo.");

            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task EditAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}