using Microsoft.EntityFrameworkCore;
using TicTacToe.Core.Models;
using TicTacToe.DataAccess.Entities;

namespace TicTacToe.DataAccess.Repositories
{
    public class GamesRepository : IGamesRepository
    {
        private readonly TicTacToeDbContext _context;
        public GamesRepository(TicTacToeDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Create(Game game)
        {
            var gameEntity = new GameEntity
            {
                Id = game.Id,
                State = game.State,
                Date = game.Date,
                Difficulty = game.Difficulty,
                UserId = game.UserId,
                Finished = game.Finished,
                Result = game.Result,
            };

            await _context.Games.AddAsync(gameEntity);
            await _context.SaveChangesAsync();

            return gameEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string State, uint Difficulty)
        {
            await _context.Games
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(g => g.State, g => State)
                    .SetProperty(g => g.Difficulty, Difficulty)
                    );

            return id;
        }
    }
}
