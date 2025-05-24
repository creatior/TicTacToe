namespace TicTacToe.API.Contracts
{
    public record GameStatesResponse(
        Guid Id,
        string State,
        DateTime Date,
        uint Difficulty,
        bool Finished
        );
}
