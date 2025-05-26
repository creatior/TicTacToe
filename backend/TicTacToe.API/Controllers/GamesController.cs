using Microsoft.AspNetCore.Mvc;
using TicTacToe.API.Contracts;
using TicTacToe.Application.Services;
using TicTacToe.Core.Models;

namespace TicTacToe.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGamesService _gameService;

        public GamesController(IGamesService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateGame([FromBody] GamesRequest request)
        {
            var (game, error) = Game.Create(
                Guid.NewGuid(),
                request.State,
                request.Difficulty,
                request.UserId);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var gameStateId = await _gameService.CreateGame(game);

            return Ok(gameStateId);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GamesResponse>> GetGame(Guid id)
        {
            var game = await _gameService.GetGame(id);

            if (game == null)
            {
                return NotFound();
            }

            var response = new GamesResponse(
                Id: game.Id,
                State: game.State,
                Difficulty: game.Difficulty
            );

            return Ok(response);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateGame(Guid id, [FromBody] GamesRequest request)
        {
            var gameStateId = await _gameService.UpdateGame(
                id,
                request.State,
                request.Difficulty,
                request.Finished,
                request.UserId,
                request.Result
            );

            return Ok(gameStateId);
        }

        [HttpGet("unfinished/{userId:guid}")]
        public async Task<ActionResult<GamesResponse>> GetUnfinishedGameByUser(Guid userId)
        {
            // Получаем первую незавершенную игру пользователя
            var game = await _gameService.GetUnfinishedGameByUser(userId);

            if (game == null)
            {
                return NotFound();
            }

            var response = new GamesResponse(
                Id: game.Id,
                State: game.State,
                Difficulty: game.Difficulty
            );

            return Ok(response);
        }

        [HttpGet("recent-finished/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<GamesResponse>>> GetRecentFinishedGames(Guid userId)
        {
            // Получаем 10 последних завершенных игр пользователя
            var games = await _gameService.GetRecentFinishedGames(userId, 10);

            var response = games.Select(game => new GamesResponse(
                Id: game.Id,
                State: game.State,
                Difficulty: game.Difficulty
            ));

            return Ok(response);
        }
    }
}   