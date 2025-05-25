using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.DataAccess;
using TicTacToe.Functional;

namespace TicTacToe.API.Controllers
{
    [ApiController]
    [Route("api/stats")]
    public class StatsController : ControllerBase
    {
        private readonly TicTacToeDbContext _context;

        public StatsController(TicTacToeDbContext context)
        {
            _context = context;
        }


        // GET api/stats/global
        [HttpGet("global")]
        public IActionResult GetGlobalStats()
        {
            var stats = StatsQueries.getGlobalStats(_context);
            return Ok(new
            {
                Wins = stats.Item1,
                Losses = stats.Item2,
                Draws = stats.Item3
            });
        }


        // GET api/stats/difficulty
        [HttpGet("difficulty")]
        public IActionResult GetDifficultyStats()
        {
            var stats = StatsQueries.getGamesByDifficulty(_context)
                        .Select(t => new { Difficulty = t.Item1, Count = t.Item2 });
            return Ok(stats);
        }

        // GET api/stats/user/{userId}/max-win-streak
        [HttpGet("user/{userId}/max-win-streak")]
        public IActionResult GetMaxWinStreak(Guid userId)
        {
            var streak = StatsQueries.getMaxWinStreak(_context, userId);
            return Ok(new { MaxWinStreak = streak });
        }
        

        // GET api/stats/user/{userId}/difficulty
        [HttpGet("user/{userId}/difficulty")]
        public IActionResult GetUserStatsByDifficulty(Guid userId)
        {
            var stats = StatsQueries.getUserStatsByDifficulty(_context, userId)
                        .Select(t => new
                        {
                            Difficulty = t.Item1,
                            Wins = t.Item2,
                            Losses = t.Item3,
                            Draws = t.Item4
                        });

            return Ok(stats);
        }

    }
}