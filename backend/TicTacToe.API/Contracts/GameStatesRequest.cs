namespace TicTacToe.API.Contracts
{
    public record GameStatesRequest(
        string State,
        uint Difficulty,
        bool Finished,
        Guid UserId
        );
}
