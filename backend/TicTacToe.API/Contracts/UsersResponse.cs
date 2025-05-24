namespace TicTacToe.API.Contracts
{
    public record UsersResponse(
        Guid Id,
        string Username,
        string Password
        );
}
