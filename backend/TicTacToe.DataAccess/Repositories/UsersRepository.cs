using Microsoft.EntityFrameworkCore;
using TicTacToe.Core.Models;
using TicTacToe.DataAccess.Entities;

namespace TicTacToe.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly TicTacToeDbContext _context;
        public UsersRepository(TicTacToeDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Create(User user)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.Id;
        }
        public async Task<User?> GetByUsername(string username)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);

            if (userEntity == null)
            {
                return null;
            }

            var (user, error) = User.Create(userEntity.Id, userEntity.Username, userEntity.Password);
            if (!string.IsNullOrEmpty(error))
            {
                return null;
            }
            return user;
        }
    }
}
