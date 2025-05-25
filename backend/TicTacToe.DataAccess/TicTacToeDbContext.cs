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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameStateEntity>()
                .HasOne(g => g.User)
                .WithMany()
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
