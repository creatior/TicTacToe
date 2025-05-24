using Microsoft.EntityFrameworkCore;
using TicTacToe.DataAccess.Entities;

namespace TicTacToe.DataAccess
{
    public class TicTacToeDbContext : DbContext
    {
        public TicTacToeDbContext(DbContextOptions<TicTacToeDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<GameStateEntity> GameStates {  get; set; }
    }
}
