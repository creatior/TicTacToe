namespace TicTacToe.API.Contracts
{
    public record GamesResponse(
        Guid Id,
        string State,
        uint? Difficulty
        );
}
