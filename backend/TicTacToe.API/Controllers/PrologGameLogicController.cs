using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace TicTacToe.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> _logger;
        private readonly IWebHostEnvironment _env;

        public BotController(ILogger<BotController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        [HttpPost("move")]
        public async Task<ActionResult<BotMoveResponse>> GetBotMove([FromBody] BotMoveRequest request)
        {
            // Валидация входных данных
            if (request.Board.Length != 16)
                return BadRequest("Invalid board size. Must contain exactly 16 elements");

            if (request.Difficulty < 1 || request.Difficulty > 4)
                return BadRequest("Difficulty must be between 1 and 4");

            // Полный путь к файлу main.pl
            var prologFilePath = Path.Combine(AppContext.BaseDirectory, "Prolog", "main.pl");
            
            if (!System.IO.File.Exists(prologFilePath))
            {
                _logger.LogError($"Prolog file not found at: {prologFilePath}");
                return StatusCode(500, "Prolog logic file not found");
            }

            // Проверка текущего состояния до хода бота
            var preCheckResult = await ExecutePrologQuery(
                prologFilePath,
                BuildCheckQuery(request.Board), 
                "Check initial state");
            
            if (preCheckResult.Contains("player_wins"))
                return Ok(new BotMoveResponse(request.Board, "PlayerWin"));
            
            if (preCheckResult.Contains("game_draw"))
                return Ok(new BotMoveResponse(request.Board, "Draw"));

            // Получение хода бота
            var query = BuildMoveQuery(request.Board, request.Difficulty);
            var result = await ExecutePrologQuery(prologFilePath, query, "Get bot move");
            
            // Обработка результата из Prolog
            try
            {
                var newBoard = ParsePrologOutput(result);
                var gameStatus = await DetermineGameStatus(prologFilePath, newBoard);
                return Ok(new BotMoveResponse(newBoard, gameStatus));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Prolog output");
                return StatusCode(500, "Error processing bot move");
            }
        }

        private async Task<string> ExecutePrologQuery(string prologFilePath, string query, string operationName)
        {
            try
            {
                // Типичные пути установки SWI-Prolog на Windows
                var swiPaths = new[]
                {
                    @"C:\Program Files\swipl\bin\swipl.exe",
                    @"C:\Program Files (x86)\swipl\bin\swipl.exe",
                    @"C:\swipl\bin\swipl.exe",
		    @"swipl",
                };

                var swiPath = swiPaths.FirstOrDefault(System.IO.File.Exists) ?? "swipl";

                var procStartInfo = new ProcessStartInfo
                {
                    FileName = swiPath,
                    Arguments = $"-q -s \"{prologFilePath}\" -g \"{query}\" -t halt",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    WorkingDirectory = Path.GetDirectoryName(prologFilePath)
                };

                _logger.LogInformation($"Executing Prolog: {procStartInfo.FileName} {procStartInfo.Arguments}");

                using var process = new Process { StartInfo = procStartInfo };
                process.Start();
                
                var output = await process.StandardOutput.ReadToEndAsync();
                var error = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();

                _logger.LogInformation($"Prolog output: {output}");
                _logger.LogInformation($"Prolog error: {error}");

                if (process.ExitCode != 0)
                {
                    throw new Exception($"Prolog error: {error}");
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing Prolog query for {operationName}");
                throw;
            }
        }

        private string BuildCheckQuery(int[] board)
        {
            var boardStr = $"[{string.Join(',', board)}]";
            return $"(winner({boardStr},1) -> write('player_wins'); " +
                   $"(is_draw({boardStr}) -> write('game_draw'); write('continue')))";
        }

        private string BuildMoveQuery(int[] board, int difficulty)
        {
            var boardStr = $"[{string.Join(',', board)}]";
            return difficulty switch
            {   
                1 => $"entry_point_primitive({boardStr}, NewBoard), write(NewBoard)",
                2 => $"entry_point_easy({boardStr}, NewBoard), write(NewBoard)",
                3 => $"entry_point_medium({boardStr}, NewBoard), write(NewBoard)",
                4 => $"entry_point_hard({boardStr}, NewBoard), write(NewBoard)",
                _ => throw new ArgumentException("Invalid difficulty level")
            };
        }

        private int[] ParsePrologOutput(string output)
        {
            try
            {
                // Удаляем все пробелы и переносы строк
                var cleanOutput = output.Trim()
                    .Replace("\n", "").Replace("\r", "")
                    .Replace("[", "").Replace("]", "")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries);

                return cleanOutput.Select(int.Parse).ToArray();
            }
            catch
            {
                throw new FormatException("Invalid Prolog output format");
            }
        }

        private async Task<string> DetermineGameStatus(string prologFilePath, int[] board)
        {
            var result = await ExecutePrologQuery(
                prologFilePath,
                $"(winner({JsonSerializer.Serialize(board)},2) -> write('bot_wins'); " +
                $"(is_draw({JsonSerializer.Serialize(board)}) -> write('game_draw'); write('continue')))", 
                "Determine game status");

            return result.Contains("bot_wins") ? "BotWin" :
                   result.Contains("game_draw") ? "Draw" : "Continue";
        }
    }

    public class BotMoveRequest
    {
        public int[] Board { get; set; }
        public int Difficulty { get; set; }
    }

    public class BotMoveResponse
    {
        public int[] NewBoard { get; set; }
        public string GameStatus { get; set; } // "PlayerWin", "BotWin", "Draw", "Continue"

        public BotMoveResponse(int[] newBoard, string gameStatus)
        {
            NewBoard = newBoard;
            GameStatus = gameStatus;
        }
    }
}
