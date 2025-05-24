namespace TicTacToe.DataAccess.Entities
{
    public class GameStateEntity
    {
        public Guid Id { get; set; }
        public string State { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public uint Difficulty { get; set; }
        public bool Finished { get; set; }
    }
}
