namespace TicTacToe.Core.Models
{
    public class User
    {
        public const int MAX_USERNAME_LENGTH = 20;

        private User(Guid id, string username, string password, string email)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
        }

        public Guid Id { get; }
        public string Username { get; } = string.Empty;
        public string Password { get; } = string.Empty;
        public string Email { get; } = string.Empty;

        public static (User User, string Error) Create(Guid id, string username, string password, string email)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(username) || username.Length > MAX_USERNAME_LENGTH)
            {
                error = "Username cannot be empty or longer than 20 symbols";
            }

            var user = new User(id, username, password, email);

            return (user, error);
        }
    }
}
