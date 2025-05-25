namespace TicTacToe.Core.Models
{
    public class Game
    {
        public Game(Guid id, string? state, DateTime date, uint? difficulty, Guid? userId)
        {
            Id = id;
            State = state;
            Date = date;
            Difficulty = difficulty;
            UserId = userId;
            Finished = false;
            Result = 0;
            
        }

        public Guid Id { get; }
        public string? State { get; } = string.Empty;
        public DateTime Date { get; }
        public uint? Difficulty { get; }
        public Guid? UserId { get; }
        public bool Finished { get; }
        public uint Result { get; }

        public static (Game Game, string error) Create(Guid Id, string? State, uint? Difficulty, Guid? UserId)
        {
            var error = string.Empty;

            if (State != null && State.Length != 16)
            {
                error = "Field size must be 16 cells";
            }
            if (Difficulty > 4)
            {
                error = "Incorrect difficulty";
            }

            var curTime = DateTime.UtcNow;

            var game = new Game(Id, State, curTime, Difficulty, UserId);

            return (game, error);
        }
    }
}
