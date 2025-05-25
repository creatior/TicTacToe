using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.DataAccess.Entities;

namespace TicTacToe.DataAccess.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<GameEntity>
    {
        public void Configure(EntityTypeBuilder<GameEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.State.Length == 16)
                .IsRequired();

            builder.Property(x => x.Difficulty)
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.Date)
           .HasConversion(
               v => v.ToUniversalTime(),
               v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
           );
        }
    }
}
