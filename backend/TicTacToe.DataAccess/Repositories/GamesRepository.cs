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

        public async Task<Game?> Get(Guid id)
        {
            var gameEntity = await _context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (gameEntity == null)
            {
                return null;
            }

            var (game, error) = Game.Create(gameEntity.Id, gameEntity.State, gameEntity.Difficulty, gameEntity.UserId);
            if (!string.IsNullOrEmpty(error))
            {
                return null;
            }
            return game;
        }
        public async Task<Guid?> Update(Guid id, string? State, uint? Difficulty, bool? Finished, Guid? UserId, uint? Result)
        {
            await _context.Games
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(g => g.State, g => State ?? g.State)
                    .SetProperty(g => g.Difficulty, g => Difficulty ?? g.Difficulty)
                    .SetProperty(g => g.Finished, g => Finished ?? g.Finished)
                    .SetProperty(g => g.UserId, g => UserId ?? g.UserId)
                    .SetProperty(g => g.Result, g => Result ?? g.Result)
                );

            return id;
        }

        public async Task<Game?> GetUnfinishedByUser(Guid userId)
        {
            var gameEntity = await _context.Games
                .AsNoTracking()
                .Where(g => g.UserId == userId && g.Finished == false)
                .OrderByDescending(g => g.Date)
                .FirstOrDefaultAsync();

            if (gameEntity == null)
            {
                return null;
            }

            var (game, error) = Game.Create(gameEntity.Id, gameEntity.State, gameEntity.Difficulty, gameEntity.UserId);
            return string.IsNullOrEmpty(error) ? game : null;
        }

        public async Task<IEnumerable<Game>> GetRecentFinished(Guid userId, int count)
        {
            var gameEntities = await _context.Games
                .AsNoTracking()
                .Where(g => g.UserId == userId && g.Finished == true)
                .OrderByDescending(g => g.Date)
                .Take(count)
                .ToListAsync();

            var games = new List<Game>();
            foreach (var gameEntity in gameEntities)
            {
                var (game, error) = Game.Create(gameEntity.Id, gameEntity.State, gameEntity.Difficulty, gameEntity.UserId);
                if (string.IsNullOrEmpty(error))
                {
                    games.Add(game);
                }
            }

            return games;
        }
    }
}
