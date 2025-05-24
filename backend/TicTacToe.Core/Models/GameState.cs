namespace TicTacToe.Core.Models
{
    public class GameState
    {
        public GameState(Guid id, string state, DateTime date, uint difficulty, bool finished)
        {
            Id = id;
            State = state;
            Date = date;
            Difficulty = difficulty;
            Finished = finished;
        }

        public Guid Id { get; }
        public string State { get; } = string.Empty;
        public DateTime Date { get; }
        public uint Difficulty { get; }
        public bool Finished { get; }
        
        public static (GameState GameState, string error) Create(Guid Id, string State, DateTime Date, uint difficulty, bool Finished)
        {
            var error = string.Empty;

            if (State.Length != 16)
            {
                error = "Field size must be 16 cells";
            }
            if (difficulty > 4)
            {
                error = "Incorrect difficulty";
            }

            var gameState = new GameState(Id, State, Date, difficulty, Finished);

            return (gameState, error);
        }
    }
}
