using TicTacToe.Core.Models;

namespace TicTacToe.DataAccess.Repositories
{
    public interface IUsersRepository
    {
        Task<Guid> Create(User user);
        Task<User?> GetByUsername(string username);
    }
}