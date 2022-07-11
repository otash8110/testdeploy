using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Application.Interfaces.IDataBase;
using Infrastructure.DataBaseContext;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbAccessContext _dbContext;
        public BaseRepository(DbAccessContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string[] classToInclude = null)
        {
            var filteredItem = _dbContext.Set<T>().Where(filter);
            if (classToInclude != null)
            {
                foreach(var item in classToInclude) {
                    filteredItem = filteredItem.Include(item);
                }
            }
            return await filteredItem.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
