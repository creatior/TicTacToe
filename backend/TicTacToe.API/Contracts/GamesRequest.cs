namespace TicTacToe.API.Contracts
{
    public record GamesRequest(
        string State,
        uint Difficulty,
        bool Finished,
        Guid UserId,
        uint Result
        );
}
