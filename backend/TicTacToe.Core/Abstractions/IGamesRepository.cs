using TicTacToe.Core.Models;

namespace TicTacToe.DataAccess.Repositories
{
    public interface IGamesRepository
    {
        Task<Guid> Create(Game gameState);
        Task<Guid?> Update(Guid id, string? State, uint? Difficulty, bool? Finished, Guid? UserId, uint? Result);
    }
}