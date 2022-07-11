using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Interfaces.IServices
{
    public interface IUsersService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserAsync(Expression<Func<User, bool>> predicate, string[] classToInclude = null);
        Task<string> GetUserFullName(int id);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<string> GetRolesAsync(User user);
    }
}
