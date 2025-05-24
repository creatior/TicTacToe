using TicTacToe.Core.Models;

namespace TicTacToe.DataAccess.Repositories
{
    public interface IGameStatesRepository
    {
        Task<Guid> Create(GameState gameState);
        Task<Guid> Update(Guid id, string State, bool Finished);
    }
}