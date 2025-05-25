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
        public DbSet<GameEntity> Games {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameEntity>()
                .HasOne(g => g.User)
                .WithMany()
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
