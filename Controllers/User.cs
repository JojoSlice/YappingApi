using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace YappingAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class User(Services.MongoDB mongoDb) : ControllerBase
    {
        private readonly Services.MongoDB _db = mongoDb;

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (await _db.LogIn(request.Username, request.Password))
            {
                return Ok();
            }
            return Unauthorized();
        }


        [HttpGet("GetUserId")]
        public IActionResult GetUserFromUsername(string username)
        {
            if (username != null || username != string.Empty)
            {
                var user = _db.GetUserId(username);
                if (user != null) return Ok(new { user.Id });
            }
            return Unauthorized(new {message = "User not found"});
        }

        [HttpGet("GetUserFromId")]
        public async Task<IActionResult> GetUserFromId(string userId)
        {
            Console.WriteLine("User from id körs");
            if (userId != string.Empty)
            {
                Models.User user = await _db.GetUserFromId(userId);
                Console.WriteLine(user.Username + " hämtas");
                if (user != null) return Ok(new { user });
            }
            return Unauthorized(new { message = "User not authenticated." });
        }


        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

    }
}
