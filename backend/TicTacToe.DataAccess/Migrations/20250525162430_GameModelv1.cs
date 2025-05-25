using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class GameModelv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Games",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "Result",
                table: "Games",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Games");
        }
    }
}
