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
    }
}
