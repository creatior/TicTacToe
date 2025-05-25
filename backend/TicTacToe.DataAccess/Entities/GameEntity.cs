namespace TicTacToe.DataAccess.Entities
{
    public class GameEntity
    {
        public Guid Id { get; set; }
        public string State { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public uint Difficulty { get; set; }
        public Guid UserId { get; set; } // foreign key
        public UserEntity User { get; set; } = null!;
        public bool Finished { get; set; } = false;
        public uint Result { get; set; }
    }
}
