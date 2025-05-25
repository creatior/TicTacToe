using TicTacToe.Core.Models;

namespace TicTacToe.Application.Services
{
    public interface IGamesService
    {
        Task<Guid> CreateGame(Game game);
        Task<Guid?> UpdateGame(Guid id, string? State, uint? Difficulty, bool? Finished, Guid? UserId, uint? Result);
    }
}