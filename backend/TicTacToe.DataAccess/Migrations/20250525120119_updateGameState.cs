using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicTacToe.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateGameState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "GameStates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_GameStates_UserId",
                table: "GameStates",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameStates_Users_UserId",
                table: "GameStates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameStates_Users_UserId",
                table: "GameStates");

            migrationBuilder.DropIndex(
                name: "IX_GameStates_UserId",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GameStates");
        }
    }
}
