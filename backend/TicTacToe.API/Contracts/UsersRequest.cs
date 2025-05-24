namespace TicTacToe.API.Contracts
{
    public record UsersRequest(
        string Username,
        string Password,
        string Email);
}
