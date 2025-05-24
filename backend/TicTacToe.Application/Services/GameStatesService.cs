using TicTacToe.Core.Models;
using TicTacToe.DataAccess.Repositories;

namespace TicTacToe.Application.Services
{
    public class GameStatesService : IGameStatesService
    {
        private readonly IGameStatesRepository _gameStatesRepository;
        public GameStatesService(IGameStatesRepository gameStatesRepository)
        {
            _gameStatesRepository = gameStatesRepository;
        }

        public async Task<Guid> CreateGameState(GameState gameState)
        {
            return await _gameStatesRepository.Create(gameState);
        }

        public async Task<Guid> UpdateGameState(Guid id, string State, bool Finished)
        {
            return await _gameStatesRepository.Update(id, State, Finished);
        }
    }
}
