using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.DataAccess.Entities;

namespace TicTacToe.DataAccess.Configurations
{
    public class GameStateConfiguration : IEntityTypeConfiguration<GameStateEntity>
    {
        public void Configure(EntityTypeBuilder<GameStateEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.State.Length == 16)
                .IsRequired();

            builder.Property(x => x.Difficulty)
                .HasMaxLength(4)
                .IsRequired();
        }
    }
}
