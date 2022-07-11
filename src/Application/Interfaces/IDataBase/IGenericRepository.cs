using System.Linq.Expressions;

namespace Application.Interfaces.IDataBase
{
    public interface IGenericRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string[] classToInclude = null);
        Task<List<T>> GetAllAsync();
    }
}
