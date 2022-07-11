using Application.Interfaces.IServices;
using Domain.Entities;
using Domain.Exceptions;
using Application.Interfaces.IDataBase;
using System.Linq.Expressions;

namespace Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        public UsersService(IGenericRepository<User> userRepository,
            IGenericRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            if (user.Password == password) return Task.FromResult(true);
            else return Task.FromResult(false);
        }

        public async Task<string> GetRolesAsync(User user)
        {
            var role = await _roleRepository.GetAsync(r => r.Id == user.RoleId);
            return role.Name;
        }

        public async Task<User> GetUserAsync(Expression<Func<User, bool>> predicate, string[] classToInclude = null)
        {
            var user = await _userRepository.GetAsync(predicate, classToInclude);
            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new KeyNotFoundException("No user with such ID");
            return user;
        }

        public async Task<string> GetUserFullName(int id)
        {
            var user = await GetUserByIdAsync(id);
            return user.FullName;
        }
    }
}
