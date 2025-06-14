﻿using System.Runtime.InteropServices.Marshalling;
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

        public async Task<Game?> GetGame(Guid id)
        {
            return await _gameRepository.Get(id);
        }

        public async Task<Guid?> UpdateGame(Guid id, string? State, uint? Difficulty, bool? Finished, Guid? UserId, uint? Result)
        {
            return await _gameRepository.Update(id, State, Difficulty, Finished, UserId, Result);
        }
        public async Task<Game?> GetUnfinishedGameByUser(Guid userId)
        {
            return await _gameRepository.GetUnfinishedByUser(userId);
        }

        public async Task<IEnumerable<Game>> GetRecentFinishedGames(Guid userId, int count)
        {
            return await _gameRepository.GetRecentFinished(userId, count);
        }
    }
}
