using System.Runtime.InteropServices.Marshalling;
using TicTacToe.Core.Models;
using TicTacToe.DataAccess.Repositories;

namespace TicTacToe.Application.Services
{
    public class GamesService : IGamesService
    {
        private readonly IGamesRepository _gameRepository;
        public GamesService(IGamesRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Guid> CreateGame(Game game)
        {
            return await _gameRepository.Create(game);
        }

        public async Task<Guid> UpdateGame(Guid id, string State, uint Difficulty)
        {
            return await _gameRepository.Update(id, State, Difficulty);
        }
    }
}
