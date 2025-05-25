using BCrypt.Net;

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

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User(id, username, passwordHash);

            return (user, error);
        }

        public bool VerifyPassword(string inputPassword)
        {
            Console.WriteLine("=== DEBUG VERIFY ===");
            Console.WriteLine($"Input password: '{inputPassword}'");
            Console.WriteLine($"Stored hash: '{Password}'");
            Console.WriteLine($"Hash length: {Password?.Length}");
            Console.WriteLine($"Is null: {Password == null}");

            try
            {
                bool result = BCrypt.Net.BCrypt.Verify(inputPassword.Trim(), Password);
                Console.WriteLine($"Verify result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Verify ERROR: {ex.Message}");
                return false;
            }
        }

    }
}
