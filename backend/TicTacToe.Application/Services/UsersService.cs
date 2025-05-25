using TicTacToe.Core.Models;
using TicTacToe.DataAccess.Repositories;

namespace TicTacToe.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public async Task<Guid> CreateUser(User user)
        {
            return await _usersRepository.Create(user);
        }
        public async Task<Guid?> Authenticate(string username, string password)
        {
            var user = await _usersRepository.GetByUsername(username);

            if (user == null || !user.VerifyPassword(password))
            {
                return null;
            }

            return user.Id;
        }
    }
}
