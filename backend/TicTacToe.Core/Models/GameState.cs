namespace TicTacToe.Core.Models
{
    public class GameState
    {
        public GameState(Guid id, string state, DateTime date, uint difficulty, bool finished, Guid userId)
        {
            Id = id;
            State = state;
            Date = date;
            Difficulty = difficulty;
            Finished = finished;
            UserId = userId;
        }

        public Guid Id { get; }
        public string State { get; } = string.Empty;
        public DateTime Date { get; }
        public uint Difficulty { get; }
        public bool Finished { get; }
        public Guid UserId { get; }

        public static (GameState GameState, string error) Create(Guid Id, string State, DateTime Date, uint Difficulty, bool Finished, Guid UserId)
        {
            var error = string.Empty;

            if (State.Length != 16)
            {
                error = "Field size must be 16 cells";
            }
            if (Difficulty > 4)
            {
                error = "Incorrect difficulty";
            }

            var utcDate = Date.Kind == DateTimeKind.Local ? Date.ToUniversalTime() : Date;

            var gameState = new GameState(Id, State, utcDate, Difficulty, Finished, UserId);

            return (gameState, error);
        }
    }
}
