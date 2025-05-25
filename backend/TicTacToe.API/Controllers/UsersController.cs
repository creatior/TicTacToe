using Microsoft.AspNetCore.Mvc;
using TicTacToe.API.Contracts;
using TicTacToe.Application.Services;

namespace TicTacToe.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            this._usersService = usersService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] UsersRequest request)
        {
            var (user, error) = Core.Models.User.Create(
                Guid.NewGuid(),
                request.Username,
                request.Password);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var userId = await _usersService.CreateUser(user);

            return Ok(userId);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Guid>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
            {
                return BadRequest("Username is required");
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Password is required");
            }

            var userId = await _usersService.Authenticate(request.Username, request.Password);

            if (userId == null)
            {
                return Unauthorized("Invalid username or password");
            }

            return Ok(userId);
        }
    }
}
