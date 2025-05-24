using Microsoft.AspNetCore.Mvc;
using TicTacToe.API.Contracts;
using TicTacToe.Application.Services;
using TicTacToe.Core.Models;

namespace TicTacToe.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameStatesController : ControllerBase
    {
        private readonly IGameStatesService _gameStatesService;

        public GameStatesController(IGameStatesService gameStateService)
        {
            _gameStatesService = gameStateService;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateGameState([FromBody] GameStatesRequest request)
        {
            var (gameState, error) = GameState.Create(
                Guid.NewGuid(),
                request.State,
                DateTime.Now,
                request.Difficulty,
                request.Finished);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var gameStateId = await _gameStatesService.CreateGameState(gameState);

            return Ok(gameStateId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateGameState(Guid id, [FromBody] GameStatesRequest request)
        {
            var gameStateId = await _gameStatesService.UpdateGameState(id, request.State, request.Finished);

            return Ok(gameStateId);
        }
    }
}