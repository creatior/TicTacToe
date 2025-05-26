using TicTacToe.Core.Models;

namespace TicTacToe.DataAccess.Repositories
{
    public interface IGamesRepository
    {
        Task<Guid> Create(Game gameState);
        Task<Game?> Get(Guid gameId);
        Task<Guid?> Update(Guid id, string? State, uint? Difficulty, bool? Finished, Guid? UserId, uint? Result);
        Task<Game?> GetUnfinishedByUser(Guid userId);
        Task<IEnumerable<Game>> GetRecentFinished(Guid userId, int count);
    }
}