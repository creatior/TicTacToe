using TicTacToe.Core.Models;
using TicTacToe.DataAccess.Repositories;

namespace TicTacToe.Application.Services
{
    public interface IGamesService
    {
        Task<Guid> CreateGame(Game game);
        Task<Game?> GetGame(Guid gameId);
        Task<Guid?> UpdateGame(Guid id, string? State, uint? Difficulty, bool? Finished, Guid? UserId, uint? Result);
        Task<Game?> GetUnfinishedGameByUser(Guid userId);
        Task<IEnumerable<Game>> GetRecentFinishedGames(Guid userId, int count);
    }
}