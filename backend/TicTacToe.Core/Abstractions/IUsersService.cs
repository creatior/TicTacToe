using TicTacToe.Core.Models;

namespace TicTacToe.Application.Services
{
    public interface IUsersService
    {
        Task<Guid> CreateUser(User user);
        Task<Guid?> Authenticate(string username, string password);
    }
}