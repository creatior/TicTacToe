using BCrypt.Net;
using System.Text;

namespace TicTacToe.Core.Models
{
    public class User
    {
        public const int MAX_USERNAME_LENGTH = 20;
        private const int BCRYPT_WORK_FACTOR = 12;

        private User(Guid id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public Guid Id { get; }
        public string Username { get; } = string.Empty;
        public string Password { get; } = string.Empty;

        public static (User User, string Error) Create(Guid id, string username, string password)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(username) || username.Length > MAX_USERNAME_LENGTH)
            {
                error = "Username cannot be empty or longer than 20 symbols";
            }

            if (string.IsNullOrEmpty(password))
            {
                error = "Password cannot be empty";
            }

            var user = new User(id, username, password);
            return (user, error);
        }

        public bool VerifyPassword(string inputPassword)
        {
            return inputPassword == Password;
        }

    }
}
