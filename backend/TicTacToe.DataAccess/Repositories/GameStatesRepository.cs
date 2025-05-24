using Microsoft.EntityFrameworkCore;
using TicTacToe.Core.Models;
using TicTacToe.DataAccess.Entities;

namespace TicTacToe.DataAccess.Repositories
{
    public class GameStatesRepository : IGameStatesRepository
    {
        private readonly TicTacToeDbContext _context;
        public GameStatesRepository(TicTacToeDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Create(GameState gameState)
        {
            var gameStateEntity = new GameStateEntity
            {
                Id = gameState.Id,
                State = gameState.State,
                Date = DateTime.Now,
                Difficulty = gameState.Difficulty,
                Finished = gameState.Finished
            };

            await _context.GameStates.AddAsync(gameStateEntity);
            await _context.SaveChangesAsync();

            return gameStateEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string State, bool Finished)
        {
            await _context.GameStates
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(g => g.State, g => State)
                    .SetProperty(g => g.Finished, g => Finished)
                    );

            return id;
        }
    }
}
