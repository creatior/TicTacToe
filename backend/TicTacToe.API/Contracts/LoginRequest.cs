﻿namespace TicTacToe.API.Contracts
{
    public record LoginRequest(
        string Username,
        string Password
    );
}
