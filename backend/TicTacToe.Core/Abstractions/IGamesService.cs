using TicTacToe.Core.Models;

namespace TicTacToe.Application.Services
{
    public interface IGamesService
    {
        Task<Guid> CreateGame(Game game);
        Task<Game?> GetGame(Guid gameId);
        Task<Guid?> UpdateGame(Guid id, string? State, uint? Difficulty, bool? Finished, Guid? UserId, uint? Result);
    }
}