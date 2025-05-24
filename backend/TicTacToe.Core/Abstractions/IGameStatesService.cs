using TicTacToe.Core.Models;

namespace TicTacToe.Application.Services
{
    public interface IGameStatesService
    {
        Task<Guid> CreateGameState(GameState gameState);
        Task<Guid> UpdateGameState(Guid id, string State, bool Finished);
    }
}